using Microsoft.Data.Entity;

namespace CodingMonkey.Models
{
    public class TestInput : TestObject
    {
        public string ArgumentName { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}