namespace CodingMonkey.CodeExecutorModels
{
    using System.Collections.Generic;

    public class CodeResultSummary
    {
        public bool AllTestsExecuted { get; set; }
        public bool ErrorProcessingTests { get; set; }
        public bool HasCompilerErrors { get; set; }
        public bool HasRuntimeError { get; set; }
        public IList<CodeCompilerError> CompilerErrors { get; set; }
        public CodeRuntimeError RuntimeError { get; set; }
    }
}
