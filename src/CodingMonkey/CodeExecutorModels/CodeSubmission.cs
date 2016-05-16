namespace CodingMonkey.CodeExecutorModels
{
    using System.Collections.Generic;

    public class CodeSubmission
    {
        public CodeSubmission()
        {
            this.Tests = new List<CodeTest>();
        }

        public string Code { get; set; }
        public CodeTemplate CodeTemplate { get; set; }
        public List<CodeTest> Tests { get; set; }
    }
}
