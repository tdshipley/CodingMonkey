namespace CodingMonkey.Models
{
    using System.Collections.Generic;

    public class Exercise
    {
        public Exercise()
        {
            this.Tests = new List<Test>();
        }      
        
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string Guidance { get; set; }
        public ExerciseTemplate Template { get; set; }
        public List<ExerciseExerciseCategory> ExerciseExerciseCategories { get; set; }
        public List<Test> Tests { get; set; }
    }
}