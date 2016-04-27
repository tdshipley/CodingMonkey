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
    using System.Threading;

    public struct TestInput
    {
        public string ArgumentName;
        public dynamic Value;
        public string ValueType;
    }

    public class RoslynCompiler
    {
        public IList<CompilerError> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            IList<Diagnostic> errorsFromSource = script.Compile();
            return errorsFromSource.Select(error => new CompilerError(error)).ToList();
        }

        public async Task<object> Execute(string code, string className, string mainMethodName, List<TestInput> inputs)
        {
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

            var script = CSharpScript.Create(code).ContinueWith(executionCode);

            return (await script.RunAsync()).ReturnValue;
        }
    }
}
