using System;

namespace CodingMonkey.Models
{
    public class TestInput
    {
        public int TestInputId { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }
        public string ArgumentName { get; set; }

        public Test Test { get; set; }
    }
}