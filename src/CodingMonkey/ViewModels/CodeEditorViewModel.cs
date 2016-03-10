using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coding_monkey.ViewModels
{
    public class CodeEditorViewModel
    {
        public string Code { get; set; }
        public bool HasErrors { get; set; }
        public CompilerErrorCollection Errors { get; set; }
    }
}
