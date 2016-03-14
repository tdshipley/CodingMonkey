using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace CodingMonkey.ViewModels
{
    public class CompilerErrorViewModel
    {
        public string Id { get; set; }
        public DiagnosticSeverity Severity { get; set; }
        public string Message { get; set; }
        public int LineNumberStart { get; set; }
        public int LineNumberEnd { get; set; }
        public int ColStart { get; set; }
        public int ColEnd { get; set; }
        public int ErrorLength { get; set; }
    }
}
