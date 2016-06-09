using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkey.Models.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    public class ExerciseRepository : RepositoryBase, IRepository<Exercise>
    {
        protected override IMemoryCache MemoryCache { get; set; }

        protected override CodingMonkeyContext CodingMonkeyContext { get; set; }

        protected override TimeSpan CacheEntryTimeoutValue { get; set; }

        protected override string CacheKeyPrefix { get; set; }

        protected override string AllCacheKey { get; set; }

        protected override MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        public ExerciseRepository(IMemoryCache memoryCache, CodingMonkeyContext codingMonkeyContext)
        {
            this.MemoryCache = memoryCache;
            this.CodingMonkeyContext = codingMonkeyContext;
            this.CacheEntryTimeoutValue = TimeSpan.FromHours(2);
            this.CacheKeyPrefix = typeof(Exercise).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
                                            {
                                                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
                                            };
        }

        public List<Exercise> All()
        {
            List<Exercise> exercises = new List<Exercise>();

            if (!MemoryCache.TryGetValue(this.AllCacheKey, out exercises))
            {
                exercises = CodingMonkeyContext.Exercises
                                               .Include(e => e.ExerciseExerciseCategories)
                                               .Include(e => e.Template)
                                               .ToList();

                MemoryCache.Set(this.AllCacheKey, exercises, this.DefaultCacheEntryOptions);
            }

            return exercises;
        }

        public Exercise GetById(int id)
        {
            Exercise exercise = null;

            string exerciseCacheKey = this.GetEntityCacheKey(id);

            if (!MemoryCache.TryGetValue(exerciseCacheKey, out exercise))
            {
                exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .Include(e => e.Template)
                                              .SingleOrDefault(e => e.ExerciseId == id);

                MemoryCache.Set(exerciseCacheKey, exercise, this.DefaultCacheEntryOptions);
            }

            return exercise;
        }

        public void Create(Exercise entity)
        {
            try
            {
                CodingMonkeyContext.Exercises.Add(entity);

                CodingMonkeyContext.SaveChanges();

                entity.RelateExerciseCategoriesToExerciseInMemory(entity.CategoryIds);

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create exercise category", ex);
            }

            string exerciseCacheKey = this.GetEntityCacheKey(entity.ExerciseId);
            MemoryCache.Set(exerciseCacheKey, entity, this.DefaultCacheEntryOptions);

            MemoryCache.Remove(this.AllCacheKey);
        }

        public void Update(int id, Exercise entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
