﻿namespace CodingMonkey.CodeExecutorModels
{
    using System.Collections.Generic;

    public class CodeTest
    {
        public string Description { get; set; }
        public List<CodeTestInput> Inputs { get; set; }
        public CodeTestExpectedOutput ExpectedOutput { get; set; }
        public object ActualOutput { get; set; }
        public CodeTestResult Result { get; set; }
    }
}
