namespace CodingMonkey.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public List<int> CategoryIds { get; set; } 

        public void RelateExerciseCategoriesToExerciseInMemory(List<int> categoryIds)
        {
            foreach (int categoryId in categoryIds)
            {
                this.ExerciseExerciseCategories.Add(
                    new ExerciseExerciseCategory()
                    {
                        ExerciseId = this.ExerciseId,
                        ExerciseCategoryId = categoryId
                    });
            }
        }
    }
}