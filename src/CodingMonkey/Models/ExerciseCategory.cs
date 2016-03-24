namespace CodingMonkey.Models
{
    public class ExerciseCategory
    {
        public int ExerciseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }
}