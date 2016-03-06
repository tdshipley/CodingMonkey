using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace CodingMonkey.CodeExecutor
{
    public class Compiler
    {
        private string Code { get; set; }
        private List<string> Assemblies { get; set; }
        private bool CompileInMemory { get; set; }

        public Compiler(string code, List<string> assemblies, bool compileInMemory)
        {
            this.Code = code;
            this.Assemblies = assemblies;
            this.CompileInMemory = compileInMemory;
        }

        public CompilerResults Compile()
        {
            var compiler = GetCSharpCompiler();
            var compilerParameters = GetCompilerParameters(this.Assemblies, this.CompileInMemory);

            return compiler.CompileAssemblyFromSource(compilerParameters, this.Code);
        }

        private CodeDomProvider GetCSharpCompiler()
        {
            const string cSharpIdentifier = "CSharp";
            return CodeDomProvider.CreateProvider(cSharpIdentifier);
        }

        private CompilerParameters GetCompilerParameters(List<string> assembliesToInclude, bool compileInMemory)
        {
            var compilerParams = new CompilerParameters(assembliesToInclude.ToArray())
            {
                GenerateInMemory = compileInMemory
            };

            return compilerParams;
        }

    }
}
