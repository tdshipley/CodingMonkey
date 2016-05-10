using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.CodeExecutor.Models
{
    public class CompilerError
    {
        public CompilerError(Diagnostic diagnosticResult)
        {
            this.Id = diagnosticResult.Id;
            this.Severity = diagnosticResult.Severity;
            this.Message = diagnosticResult.GetMessage();

            var lineSpan = diagnosticResult.Location.SourceTree.GetLineSpan(diagnosticResult.Location.SourceSpan);

            // Line nos in diagnostic result are zero based so add one
            this.StartLineNumber = lineSpan.StartLinePosition.Line + 1;
            this.EndLineNumber = lineSpan.EndLinePosition.Line + 1;

            this.ColStart = lineSpan.StartLinePosition.Character;
            this.ColEnd = lineSpan.EndLinePosition.Character;

            this.ErrorLength = diagnosticResult.Location.SourceSpan.Length;
        }

        public CompilerError(Exception ex)
        {
            this.Id = String.Empty;
            this.Message = ex.Message;
            this.Severity = DiagnosticSeverity.Error;

            this.StartLineNumber = -1;
            this.EndLineNumber = -1;
            this.ColStart = -1;
            this.ColEnd = -1;
            this.ErrorLength = -1;

        }

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
