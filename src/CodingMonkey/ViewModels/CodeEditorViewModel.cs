using System.Collections.Generic;
using CodingMonkey.CodeExecutor.Models;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.ViewModels
{
    public class CodeEditorViewModel
    {
        public CodeEditorViewModel()
        {
            this.CompilerErrors = new List<CompilerErrorViewModel>();
            this.TestResults = new List<TestResultViewModel>();
        }
        
        public string Code { get; set; }

        public bool HasCompilerErrors { get; set; }
        public bool HasRuntimeError { get; set; }
        public IList<CompilerErrorViewModel> CompilerErrors { get; set; }
        public RuntimeErrorViewModel RuntimeError { get; set; } 
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
