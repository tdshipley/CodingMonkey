using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using CodingMonkey.CodeExecutor.Models;

namespace CodingMonkey.CodeExecutor
{
    public class RoslynCompiler
    {
        public static IList<CompilerError> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            IList<Diagnostic> errorsFromSource = script.Compile();
            return errorsFromSource.Select(error => new CompilerError(error)).ToList();
        }
    }
}
