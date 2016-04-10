﻿namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    public class ExerciseCategoryViewModel
    {
        public ExerciseCategoryViewModel()
        {
            ExerciseIds = new List<int>();
        }
        
        public int Id { get; set; }
        public ICollection<int> ExerciseIds { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
