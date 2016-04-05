namespace CodingMonkey.Models
{
    using System.Collections.Generic;
    
    public class ExerciseCategory
    {
        public int ExerciseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ExerciseExerciseCategory> ExerciseExerciseCategories { get; set; }
    }
}