using System.Collections.Generic;
using CodingMonkey.CodeExecutor.Models;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.ViewModels
{
    public class CodeEditorViewModel
    {
        public string Code { get; set; }
        public bool HasErrors { get; set; }
        public IList<CompilerErrorViewModel> Errors { get; set; }
    }
}
