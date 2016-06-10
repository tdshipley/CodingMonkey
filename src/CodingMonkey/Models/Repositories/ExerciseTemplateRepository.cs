namespace CodingMonkey.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Unlike other repository classes due to the one to one relationship
    /// between a Exercise and Exercise Template this repository uses the
    /// EXERCISE id for the cache key and anywhere where id is requested.
    /// </summary>
    public class ExerciseTemplateRepository : RepositoryBase, IRepository<ExerciseTemplate>
    {
        protected override IMemoryCache MemoryCache { get; set; }

        protected override CodingMonkeyContext CodingMonkeyContext { get; set; }

        private CodingMonkeyRepositoryContext CodingMonkeyRepositoryContext { get; set; }

        protected override TimeSpan CacheEntryTimeoutValue { get; set; }

        protected override string CacheKeyPrefix { get; set; }

        protected override string AllCacheKey { get; set; }

        protected override MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        public ExerciseTemplateRepository(IMemoryCache memoryCache, CodingMonkeyContext codingMonkeyContext, CodingMonkeyRepositoryContext codingMonkeyRepositoryContext)
        {
            this.MemoryCache = memoryCache;
            this.CodingMonkeyContext = codingMonkeyContext;
            this.CodingMonkeyRepositoryContext = codingMonkeyRepositoryContext;

            this.CacheEntryTimeoutValue = TimeSpan.FromHours(2);
            this.CacheKeyPrefix = typeof(ExerciseCategory).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
            };
        }

        public List<ExerciseTemplate> All()
        {
            List<ExerciseTemplate> exerciseTemplates = new List<ExerciseTemplate>();

            if (!MemoryCache.TryGetValue(this.AllCacheKey, out exerciseTemplates))
            {
                exerciseTemplates = CodingMonkeyContext.ExerciseTemplates
                                                       .Include(e => e.Exercise)
                                                       .ToList();

                MemoryCache.Set(this.AllCacheKey, exerciseTemplates, this.DefaultCacheEntryOptions);
            }

            return exerciseTemplates;
        }

        /// <summary>
        /// Finds a Exercise Template for a given EXERCISE id
        /// </summary>
        /// <param name="id">The EXERCISE id which the Exercise Template belongs to</param>
        /// <returns></returns>
        public ExerciseTemplate GetById(int id)
        {
            ExerciseTemplate exerciseTemplate = null;

            string exerciseTemplateCacheKey = this.GetEntityCacheKey(id);

            if (!MemoryCache.TryGetValue(exerciseTemplateCacheKey, out exerciseTemplate))
            {
                exerciseTemplate = CodingMonkeyContext.ExerciseTemplates
                                                      .Include(e => e.Exercise)
                                                      .SingleOrDefault(e => e.Exercise.ExerciseId == id);

                MemoryCache.Set(exerciseTemplateCacheKey, exerciseTemplate, this.DefaultCacheEntryOptions);
            }

            return exerciseTemplate;
        }

        public ExerciseTemplate Create(ExerciseTemplate entity)
        {
            Exercise relatedExercise = null;

            try
            {
                relatedExercise = CodingMonkeyRepositoryContext.ExerciseRepository
                                                               .GetById(entity.ExerciseForeignKey);

                if(relatedExercise == null) throw new ArgumentException("Could not find related exercise to exercise template");

                relatedExercise.Template = entity;

                relatedExercise = CodingMonkeyRepositoryContext.ExerciseRepository
                                                               .Update(relatedExercise.ExerciseId, relatedExercise);
                entity = relatedExercise.Template;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create exercise category", ex);
            }

            string exerciseTemplateCacheKey = this.GetEntityCacheKey(relatedExercise.ExerciseId);
            MemoryCache.Set(exerciseTemplateCacheKey, entity, this.DefaultCacheEntryOptions);

            MemoryCache.Remove(this.AllCacheKey);

            return entity;
        }

        public ExerciseTemplate Update(int id, ExerciseTemplate entity)
        {
            Exercise relatedExercise = null;

            try
            {
                relatedExercise = this.CodingMonkeyRepositoryContext.ExerciseRepository
                                                                    .GetById(id);

                if (relatedExercise == null) throw new ArgumentException("Exercise Template to update not found.");

                relatedExercise.Template = entity;

                relatedExercise = this.CodingMonkeyRepositoryContext.ExerciseRepository
                                                                    .Update(relatedExercise.ExerciseId, relatedExercise);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update exercise category", ex);
            }

            MemoryCache.Remove(this.GetEntityCacheKey(id));
            MemoryCache.Set(this.GetEntityCacheKey(id), relatedExercise.Template);
            MemoryCache.Remove(this.AllCacheKey);

            return relatedExercise.Template;
        }

        public void Delete(int id)
        {
            Exercise relatedExercise = CodingMonkeyContext.Exercises
                                                          .SingleOrDefault(e => e.ExerciseId == id);

            try
            {
                if (relatedExercise == null) throw new ArgumentException("Related Exercise to Exercise Template to delete not found");
                CodingMonkeyContext.ExerciseTemplates.Remove(relatedExercise.Template);
                CodingMonkeyContext.SaveChanges();

                relatedExercise.Template = null;

                CodingMonkeyRepositoryContext.ExerciseRepository.Delete(relatedExercise.ExerciseId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Exercise Template", ex);
            }

            MemoryCache.Remove(this.GetEntityCacheKey(id));
            MemoryCache.Remove(this.AllCacheKey);
        }
    }
}
