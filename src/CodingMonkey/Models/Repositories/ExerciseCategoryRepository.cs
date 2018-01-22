namespace CodingMonkey.Models.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ExerciseCategoryRepository : RepositoryBase, IRepository<ExerciseCategory>
    {
        protected override IMemoryCache MemoryCache { get; set; }

        protected override CodingMonkeyContext CodingMonkeyContext { get; set; }

        protected override TimeSpan CacheEntryTimeoutValue { get; set; }

        protected override string CacheKeyPrefix { get; set; }

        protected override string AllCacheKey { get; set; }

        protected override MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        public ExerciseCategoryRepository(IMemoryCache memoryCache, CodingMonkeyContext codingMonkeyContext)
        {
            this.MemoryCache = memoryCache;
            this.CodingMonkeyContext = codingMonkeyContext;

            this.CacheEntryTimeoutValue = TimeSpan.FromHours(24);
            this.CacheKeyPrefix = typeof(ExerciseCategory).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
                                                {
                                                    AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
                                                };
        }

        public List<ExerciseCategory> All()
        {
            bool success = false;
            var exerciseCategories = this.TryGetAllInCache<ExerciseCategory>(out success);

            if(!success)
            {
                exerciseCategories = CodingMonkeyContext.ExerciseCategories
                                                .Include(e => e.ExerciseExerciseCategories)
                                                .ToList();

                MemoryCache.Set(this.AllCacheKey, exerciseCategories, this.DefaultCacheEntryOptions);
            }

            return exerciseCategories;
        }

        public ExerciseCategory GetById(int exerciseCategoryId, bool ignoreCache = false)
        {
            ExerciseCategory category = null;

            bool success = false;
            if(!ignoreCache) category = this.TryGetEntityInCacheById<ExerciseCategory>(exerciseCategoryId, out success);

            if (!success)
            {
                category = CodingMonkeyContext.ExerciseCategories
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .SingleOrDefault(e => e.ExerciseCategoryId == exerciseCategoryId);

                this.UpdateEntityInCacheById<ExerciseCategory>(exerciseCategoryId, category);
            }

            return category;
        }

        public ExerciseCategory Create(ExerciseCategory exerciseCategory)
        {
            try
            {
                CodingMonkeyContext.ExerciseCategories.Add(exerciseCategory);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create exercise category", ex);
            }

            this.CreateEntityInCacheById<ExerciseCategory>(exerciseCategory.ExerciseCategoryId, exerciseCategory);

            return exerciseCategory;
        }

        public ExerciseCategory Update(int exerciseCategoryId, ExerciseCategory newExerciseCategory)
        {
            ExerciseCategory existingExerciseCategory = this.GetById(exerciseCategoryId, true);

            if (existingExerciseCategory == null) throw new ArgumentException("Exercise category to update not found.");

            existingExerciseCategory.Name = newExerciseCategory.Name;
            existingExerciseCategory.Description = newExerciseCategory.Description;

            //try
            //{
                CodingMonkeyContext.SaveChanges();
            //}
            //catch (Exception ex)
            //{
                //throw new Exception("Failed to update exercise category", ex);
            //}

            this.UpdateEntityInCacheById<ExerciseCategory>(exerciseCategoryId, existingExerciseCategory);

            return existingExerciseCategory;
        }

        public void Delete(int exerciseCategoryId)
        {
            ExerciseCategory exerciseCategoryToDelete = CodingMonkeyContext.ExerciseCategories
                                                                           .Include(ec => ec.ExerciseExerciseCategories)
                                                                           .SingleOrDefault(e => e.ExerciseCategoryId == exerciseCategoryId);

            if (exerciseCategoryToDelete == null) throw new ArgumentException("Exercise category to delete not found");

            try
            {
                CodingMonkeyContext.ExerciseCategories.Remove(exerciseCategoryToDelete);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete exercise category", ex);
            }

            this.DeleteEntityInCacheById(exerciseCategoryId);
        }
    }
}
