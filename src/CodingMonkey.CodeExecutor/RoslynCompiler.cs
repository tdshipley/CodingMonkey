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
    public class RoslynCompiler
    {
        private static ScriptState<object> scriptState = null;

        public static IList<CompilerError> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            IList<Diagnostic> errorsFromSource = script.Compile();
            return errorsFromSource.Select(error => new CompilerError(error)).ToList();
        }

        public static object Execute(string code)
        {
            ExecuteCode(code);
            return ExecuteCode("new Numbers().ReturnNumber6()");
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
