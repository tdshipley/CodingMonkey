using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.ViewModels
{
    public class CodeEditorViewModel
    {
        public string Code { get; set; }
        public bool HasErrors { get; set; }
        public IList<Diagnostic> Errors { get; set; }
    }
}
