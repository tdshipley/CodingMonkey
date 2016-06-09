namespace CodingMonkey.Models.Repositories
{
    public class CodingMonkeyRepositoryContext : IRepositoryContext
    {
        public ExerciseCategoryRepository ExerciseCatgeoryRepository { get; set; }
        public ExerciseRepository ExerciseRepository { get; set; }
        public ExerciseTemplateRepository ExerciseTemplateRepository { get; set; }
        public TestRepository TestRepository { get; set; }

        public CodingMonkeyRepositoryContext(ExerciseCategoryRepository exerciseCatgeoryRepository,
                                             ExerciseRepository exerciseRepository,
                                             ExerciseTemplateRepository exerciseTemplateRepository,
                                             TestRepository testRepository)
        {
            this.ExerciseCatgeoryRepository = exerciseCatgeoryRepository;
            this.ExerciseRepository = exerciseRepository;
            this.ExerciseTemplateRepository = exerciseTemplateRepository;
            this.TestRepository = testRepository;
        }
    }
}
