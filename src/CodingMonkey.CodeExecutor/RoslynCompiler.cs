using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using CodingMonkey.CodeExecutor.Models;
using Newtonsoft.Json;

namespace CodingMonkey.CodeExecutor
{
    using System;

    public struct TestInput
    {
        public string ArgumentName;
        public dynamic Value;
        public string ValueType;
    }

    public struct ExecutionResult
    {
        public bool Successful;
        public object Value;
        public Exception Error;
    }

    public class RoslynCompiler
    {
        public PreExecutionSecurity Security { get; set; }

        public RoslynCompiler()
        {
            this.Security = new PreExecutionSecurity();
        }

        public IList<CompilerError> Compile(string code)
        {
            code = this.Security.SanitiseCode(code);

            var script = CSharpScript.Create(code);
            IList<Diagnostic> errorsFromSource = script.Compile();
            return errorsFromSource.Select(error => new CompilerError(error)).ToList();
        }

        public async Task<ExecutionResult> ExecuteAsync(string code, string className, string mainMethodName, List<TestInput> inputs)
        {
            code = this.Security.SanitiseCode(code);

            // Statements need a return in front of them to get the value see:
            // https://github.com/dotnet/roslyn/issues/5279
            string executionCode = $"return new {className}().{mainMethodName}(";

            foreach(var input in inputs)
            {
                if(input.ValueType == "String")
                {
                    executionCode += $"{input.ArgumentName}: \"{input.Value.ToString()}\",";
                }
                else
                {
                    executionCode += $"{input.ArgumentName}: {input.Value.ToString()},";
                }
            }

            executionCode = executionCode.TrimEnd(',') + ");";


            try
            {
                var script = CSharpScript.Create(code).ContinueWith(executionCode);

                object returnValue = (await script.RunAsync()).ReturnValue;

                return new ExecutionResult() { Successful = true, Value = returnValue, Error = null };
            }
            catch (Exception ex)
            {
                return new ExecutionResult() { Successful = false, Value = null, Error = ex };
                throw;
            }
        }
    }
}
