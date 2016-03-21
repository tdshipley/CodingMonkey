using Microsoft.Data.Entity;
using System;

namespace CodingMonkey.Models
{
    public class TestObject
    {
        public int TestObjectId { get; set; }
        public Type ValueType { get; set; }
        public object Value { get; set; }
    }
}
