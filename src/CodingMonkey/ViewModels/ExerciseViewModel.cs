namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    public class ExerciseViewModel
    {
        public ExerciseViewModel()
        {
            this.CategoryIds = new List<int>();
        }
        
        public int Id { get; set; }
        public int ExerciseTemplateId { get; set; }
        public List<int> CategoryIds { get; set; }
        public string Name { get; set; }
        public string Guidance { get; set; }
    }
}
