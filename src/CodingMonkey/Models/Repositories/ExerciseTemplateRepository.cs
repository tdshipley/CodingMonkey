﻿namespace CodingMonkey.Models.Repositories
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
    public class ExerciseTemplateRepository : RepositoryBase, IChildRepository<ExerciseTemplate>
    {
        protected override IMemoryCache MemoryCache { get; set; }

        protected override CodingMonkeyContext CodingMonkeyContext { get; set; }

        protected override TimeSpan CacheEntryTimeoutValue { get; set; }

        protected override string CacheKeyPrefix { get; set; }

        protected override string AllCacheKey { get; set; }

        protected override MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        public ExerciseTemplateRepository(IMemoryCache memoryCache, CodingMonkeyContext codingMonkeyContext)
        {
            this.MemoryCache = memoryCache;
            this.CodingMonkeyContext = codingMonkeyContext;

            this.CacheEntryTimeoutValue = TimeSpan.FromHours(24);
            this.CacheKeyPrefix = typeof(ExerciseTemplate).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
            };
        }

        public List<ExerciseTemplate> All(int exerciseId)
        {
            bool success = false;
            List<ExerciseTemplate> exerciseTemplates = new List<ExerciseTemplate>();

            exerciseTemplates = this.TryGetAllInCache<ExerciseTemplate>(out success);

            if (!success)
            {
                exerciseTemplates = CodingMonkeyContext.ExerciseTemplates
                                                       .Include(e => e.Exercise)
                                                       .Where(et => et.ExerciseForeignKey == exerciseId)
                                                       .ToList();

                MemoryCache.Set(this.AllCacheKey, exerciseTemplates, this.DefaultCacheEntryOptions);
            }

            return exerciseTemplates;
        }

        public ExerciseTemplate GetById(int exerciseId, bool ignoreCache = false)
        {
            bool success = false;
            ExerciseTemplate exerciseTemplate = null;

            if(!ignoreCache) exerciseTemplate = this.TryGetEntityInCacheById<ExerciseTemplate>(exerciseId, out success);

            if (!success)
            {
                exerciseTemplate = CodingMonkeyContext.ExerciseTemplates
                                                      .Include(e => e.Exercise)
                                                      .SingleOrDefault(e => e.Exercise.ExerciseId == exerciseId);

                this.UpdateEntityInCacheById<ExerciseTemplate>(exerciseId, exerciseTemplate);
            }

            return exerciseTemplate;
        }

        public ExerciseTemplate Create(int exerciseId, ExerciseTemplate entity)
        {
            Exercise relatedExercise = null;

            try
            {
                relatedExercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .Include(e => e.Template)
                                              .SingleOrDefault(e => e.ExerciseId == exerciseId);

                if(relatedExercise == null) throw new ArgumentException("Could not find related exercise to exercise template");

                relatedExercise.Template = entity;

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create exercise template", ex);
            }

            this.CreateEntityInCacheById<ExerciseTemplate>(relatedExercise.ExerciseId, relatedExercise.Template);

            return entity;
        }

        public ExerciseTemplate Update(int exerciseId, int exerciseTemplateId, ExerciseTemplate entity)
        {
            Exercise relatedExercise = null;

            try
            {
                relatedExercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .Include(e => e.Template)
                                              .SingleOrDefault(e => e.ExerciseId == exerciseId);

                if (relatedExercise == null) throw new ArgumentException("Exercise Template to update not found.");

                relatedExercise.Template.ClassName = entity.ClassName;
                relatedExercise.Template.InitialCode = entity.InitialCode;
                relatedExercise.Template.MainMethodName = entity.MainMethodName;
                relatedExercise.Template.MainMethodSignature = entity.MainMethodSignature;

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update exercise template", ex);
            }

            this.UpdateEntityInCacheById<ExerciseTemplate>(exerciseId, relatedExercise.Template);

            return relatedExercise.Template;
        }

        public void Delete(int exerciseId)
        {
            Exercise relatedExercise = CodingMonkeyContext.Exercises
                                                          .SingleOrDefault(e => e.ExerciseId == exerciseId);

            try
            {
                if (relatedExercise == null) throw new ArgumentException("Related Exercise to Exercise Template to delete not found");
                CodingMonkeyContext.ExerciseTemplates.Remove(relatedExercise.Template);
                relatedExercise.Template = null;

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Exercise Template", ex);
            }

            this.DeleteEntityInCacheById(exerciseId);
        }
    }
}
