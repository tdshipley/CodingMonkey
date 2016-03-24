namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    public class ExerciseViewModel
    {
        public string Name { get; set; }
        public string Guidance { get; set; }
        public ExerciseTemplateViewModel Template { get; set; }
        public List<ExerciseCategoryViewModel> Categories { get; set; }
        public List<TestViewModel> Tests { get; set; }  
    }
}
