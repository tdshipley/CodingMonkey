using System;

namespace CodingMonkey.Models
{
    public class TestInput
    {
        public int TestInputId { get; set; }
        public string ValueType { get; set; }
        public string Value { get; set; }
        public string ArgumentName { get; set; }

        public Test Test { get; set; }
    }
}