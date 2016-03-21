using Microsoft.Data.Entity;

namespace CodingMonkey.Models
{
    public class TestOutput : TestObject
    {
        public int TestForeignKey { get; set; }
        public Test Test { get; set; }
    }
}
