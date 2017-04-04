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
            this.CacheEntryTimeoutValue = TimeSpan.FromHours(24);
            this.CacheKeyPrefix = typeof(Exercise).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
                                            {
                                                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
                                            };
        }

        public List<Exercise> All()
        {
            bool success = false;
            List<Exercise> exercises = new List<Exercise>();

            exercises = this.TryGetAllInCache<Exercise>(out success);

            if (!success)
            {
                exercises = CodingMonkeyContext.Exercises
                                               .Include(e => e.ExerciseExerciseCategories)
                                               .Include(e => e.Template)
                                               .ToList();

                MemoryCache.Set(this.AllCacheKey, exercises, this.DefaultCacheEntryOptions);
            }

            return exercises;
        }

        public Exercise GetById(int exerciseId, bool ignoreCache = false)
        {
            bool success = false;
            Exercise exercise = null;

            if(!ignoreCache) exercise = this.TryGetEntityInCacheById<Exercise>(exerciseId, out success);

            if (!success)
            {
                exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .Include(e => e.Template)
                                              .SingleOrDefault(e => e.ExerciseId == exerciseId);

                this.UpdateEntityInCacheById<Exercise>(exerciseId, exercise);
            }

            return exercise;
        }

        public Exercise Create(Exercise entity)
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
                throw new Exception("Failed to create exercise", ex);
            }

            this.CreateEntityInCacheById<Exercise>(entity.ExerciseId, entity);

            return entity;
        }

        public Exercise Update(int exerciseId, Exercise entity)
        {
            Exercise existingExercise = this.GetById(exerciseId, true);

            if (existingExercise == null) throw new ArgumentException("Exercise to update not found.");

            try
            {
                CodingMonkeyContext.ExerciseExerciseCategories.RemoveRange(existingExercise.ExerciseExerciseCategories);

                CodingMonkeyContext.SaveChanges();

                existingExercise.Name = entity.Name;
                existingExercise.Guidance = entity.Guidance;
                existingExercise.ExerciseExerciseCategories = entity.ExerciseExerciseCategories;

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update exercise", ex);
            }

            this.UpdateEntityInCacheById<Exercise>(exerciseId, existingExercise);

            return existingExercise;
        }

        public void Delete(int exerciseId)
        {
            Exercise exerciseToDelete = CodingMonkeyContext.Exercises
                                                           .Include(e => e.ExerciseExerciseCategories)
                                                           .SingleOrDefault(e => e.ExerciseId == exerciseId);

            if (exerciseToDelete == null) throw new ArgumentException("Exercise to delete not found");

            try
            {
                CodingMonkeyContext.Exercises.Remove(exerciseToDelete);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete exercise", ex);
            }

            this.DeleteEntityInCacheById(exerciseId);
        }
    }
}
