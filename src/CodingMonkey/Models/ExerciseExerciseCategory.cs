namespace CodingMonkey.Models
{
    public class ExerciseExerciseCategory
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public int ExerciseCategoryId { get; set; }
        public ExerciseCategory ExerciseCategory { get; set; }
    }
}