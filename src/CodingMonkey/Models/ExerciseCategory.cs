using Microsoft.Data.Entity;
using System.Collections.Generic;

namespace CodingMonkey.Models
{
    public class ExerciseCategory
    {
        public int ExerciseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}