namespace CodingMonkey.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TestInput
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestInputId { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
        public string ArgumentName { get; set; }

        public Test Test { get; set; }
    }
}