namespace CodingMonkey.CodeExecutorModels
{
    using Microsoft.CodeAnalysis;

    public class CodeCompilerError
    {
        public string Id { get; set; }
        public DiagnosticSeverity Severity { get; set; }
        public string Message { get; set; }
        public Location Location { get; set; }
        public int StartLineNumber { get; set; }
        public int EndLineNumber { get; set; }
        public int ColStart { get; set; }
        public int ColEnd { get; set; }
        public int ErrorLength { get; set; }
    }
}
