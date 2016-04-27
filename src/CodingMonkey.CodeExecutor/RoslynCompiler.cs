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
    public struct TestInput
    {
        public string ArgumentName;
        public dynamic Value;
        public string ValueType;
    }

    public class RoslynCompiler
    {
        private static ScriptState<object> scriptState = null;

        public static IList<CompilerError> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            IList<Diagnostic> errorsFromSource = script.Compile();
            return errorsFromSource.Select(error => new CompilerError(error)).ToList();
        }

        public static object Execute(string code, string className, string mainMethodName, List<TestInput> inputs)
        {
            ExecuteCode(code);

            string executionCode = $"new {className}().{mainMethodName}(";

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

            return ExecuteCode(executionCode);
        }

        private static object ExecuteCode(string code)
        {
            scriptState = scriptState == null ? CSharpScript.RunAsync(code).Result : scriptState.ContinueWithAsync(code).Result;
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
                return scriptState.ReturnValue;
            return null;
        }
    }
}
