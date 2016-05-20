namespace CodingMonkey.Models
{
    public class TestOutput
    {
        public int TestOutputId { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }

        public int TestForeignKey { get; set; }
        public Test Test { get; set; }
    }
}
