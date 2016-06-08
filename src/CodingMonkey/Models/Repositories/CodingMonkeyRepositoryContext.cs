namespace CodingMonkey.Models.Repositories
{
    public class CodingMonkeyRepositoryContext : IRepositoryContext
    {
        public ExerciseCategoryRepository ExerciseCatgeoryRepository { get; set; }

        public CodingMonkeyRepositoryContext(ExerciseCategoryRepository exerciseCatgeoryRepository)
        {
            this.ExerciseCatgeoryRepository = exerciseCatgeoryRepository;
        }
    }
}
