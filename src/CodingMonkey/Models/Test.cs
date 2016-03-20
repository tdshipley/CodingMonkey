using Microsoft.Data.Entity;
using System.Collections.Generic;
using System;

namespace CodingMonkey.Models
{
    public class Test
    {
        public int TestId { get; set; }
        public string Description { get; set; }
        public Dictionary<string, TestValue> InputArguments { get; set; }
        public TestValue ExpectedOutput { get; set; }
    }

    public struct TestValue
    {
        public Type valType { get; set; }
        public object valObject { get; set; }
        public string valArgumentName { get; set; }
    }
}