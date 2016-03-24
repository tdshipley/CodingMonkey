namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    public class TestViewModel
    {
        public string Description { get; set; }
        public List<TestInputViewModel> TestInputs { get; set; }
        public TestOutputViewModel TestOutput { get; set; }
    }
}
