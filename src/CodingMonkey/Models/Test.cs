using Microsoft.Data.Entity;
using System.Collections.Generic;
using System;

namespace CodingMonkey.Models
{
    public class Test
    {
        public Test()
        {
            this.TestInputs = new List<TestInput>();
        }
        
        public int TestId { get; set; }
        public string Description { get; set; }
        public List<TestInput> TestInputs { get; set; }
        public TestOutput TestOutput { get; set; }

        public Exercise Exercise { get; set; }
    }
}