using Microsoft.Data.Entity;
using System;

namespace CodingMonkey.Models
{
    public class TestOutput
    {
        public int TestOutputId { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }

        public int TestForeignKey { get; set; }
        public Test Test { get; set; }
    }
}
