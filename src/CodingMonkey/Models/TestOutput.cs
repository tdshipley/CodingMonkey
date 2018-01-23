namespace CodingMonkey.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class TestOutput
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestOutputId { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }

        public int TestForeignKey { get; set; }
        public Test Test { get; set; }
    }
}
