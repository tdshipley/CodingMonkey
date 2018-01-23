namespace CodingMonkey.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExerciseCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ExerciseExerciseCategory> ExerciseExerciseCategories { get; set; }
    }
}