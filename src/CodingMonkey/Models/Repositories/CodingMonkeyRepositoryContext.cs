namespace CodingMonkey.Models.Repositories
{
    public class CodingMonkeyRepositoryContext : IRepositoryContext
    {
        public ExerciseCategoryRepository ExerciseCatgeoryRepository { get; set; }
        public ExerciseRepository ExerciseRepository { get; set; }

        public CodingMonkeyRepositoryContext(ExerciseCategoryRepository exerciseCatgeoryRepository,
                                             ExerciseRepository exerciseRepository)
        {
            this.ExerciseCatgeoryRepository = exerciseCatgeoryRepository;
            this.ExerciseRepository = exerciseRepository;
        }
    }
}
