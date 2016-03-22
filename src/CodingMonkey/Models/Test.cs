using System.Collections.Generic;

namespace CodingMonkey.Models
{
    public class Test
    {
        public int TestId { get; set; }
        public string Description { get; set; }
        public List<TestInput> TestInputs { get; set; }
        public TestOutput TestOutput { get; set; }

        public Exercise Exercise { get; set; }
    }
}