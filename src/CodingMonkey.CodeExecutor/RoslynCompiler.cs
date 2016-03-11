using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CodingMonkey.CodeExecutor
{
    public class RoslynCompiler
    {

        public static async Task<IList<Diagnostic>> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            IList<Diagnostic> errors = script.Compile();

            return errors;
        }
    }
}
