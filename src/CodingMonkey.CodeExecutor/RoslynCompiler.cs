﻿namespace CodingMonkey.CodeExecutor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using CodingMonkey.CodeExecutor.Models;

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
            var errors = errorsFromSource.Select(error => new CompilerError(error)).ToList();

            // The user doesn't see using statements added by pre security checks
            // so move the error line numbers to the right place.
            foreach (var error in errors)
            {
                error.StartLineNumber = error.StartLineNumber - this.Security.LinesOfCodeAdded;
                error.EndLineNumber = error.EndLineNumber - this.Security.LinesOfCodeAdded;
            }

            return errors;
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

                ScriptOptions scriptOptions = this.GetScriptOptions();
                var script = CSharpScript.Create(code).ContinueWith(executionCode);

                object returnValue = (await script.WithOptions(scriptOptions).RunAsync()).ReturnValue;

                return new ExecutionResult() { Successful = true, Value = returnValue, Error = null };
            }
            catch (Exception ex)
            {
                return new ExecutionResult() { Successful = false, Value = null, Error = ex };
                throw;
            }
        }

        /// <summary>
        /// Gets a script options for rosyln to use when running code.
        /// Sets up the dlls which we allow the code to run with
        /// and a custom metadata resolver to stop it trying to find
        /// missing assemblies. 
        /// </summary>
        /// <returns>Script options to use when running code</returns>
        private ScriptOptions GetScriptOptions()
        {
            ScriptOptions scriptOptions = ScriptOptions.Default;
            scriptOptions = scriptOptions.WithMetadataResolver(new CodingMonkeyMetadataReferenceResolver());

            //Add reference to mscorlib & system core
            var mscorlib = typeof(object).GetTypeInfo().Assembly;
            var systemCore = typeof(Enumerable).GetTypeInfo().Assembly;

            return scriptOptions.WithReferences(mscorlib, systemCore);
        }
    }
}
