namespace CodingMonkey.Models
{
    using System.Collections.Generic;

    public class Test
    {
        public Test()
        {
            this.TestInputs = new List<TestInput>();
        }
        
        public int TestId { get; set; }
        public string Description { get; set; }
        public List<TestInput> TestInputs { get; set; }
        public TestOutput TestOutput { get; set; }

        public Exercise Exercise { get; set; }
    }
}