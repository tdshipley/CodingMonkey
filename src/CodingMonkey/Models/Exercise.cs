using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace CodingMonkey.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Guidance { get; set; }
        public ExerciseTemplate Template { get; set; }
        public List<ExerciseCategory> Categories { get; set; }
        public List<Test> Tests { get; set; }
    }
}