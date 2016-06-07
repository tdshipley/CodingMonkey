namespace CodingMonkey.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExerciseExerciseCategory")]
    public class ExerciseExerciseCategory
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public int ExerciseCategoryId { get; set; }
        public ExerciseCategory ExerciseCategory { get; set; }
    }
}