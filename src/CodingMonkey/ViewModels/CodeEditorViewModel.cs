using System.Collections.Generic;
using CodingMonkey.CodeExecutor.Models;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.ViewModels
{
    public class CodeEditorViewModel
    {
        public CodeEditorViewModel()
        {
            Errors = new List<CompilerErrorViewModel>();
        }
        
        public string Code { get; set; }

        public bool HasErrors { get; set; }
        public IList<CompilerErrorViewModel> Errors { get; set; }
        public IList<TestResultViewModel> TestResults { get; set; }
    }

    public class TestResultViewModel
    {

        public string Description { get; set; }
        public IList<TestResultInputViewModel> Inputs { get; set; } 
        public object ExpectedOutput { get; set; }
        public object ActualOutput { get; set; }
        public bool TestPassed { get; set; }
    }

    public struct TestResultInputViewModel
    {
        public string ArgumentName { get; set; }
        public object Value { get; set; }
    }
}
