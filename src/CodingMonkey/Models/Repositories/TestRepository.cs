namespace CodingMonkey.Models.Repositories
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    public class TestRepository : RepositoryBase, IChildRepository<Test>
    {
        protected override IMemoryCache MemoryCache { get; set; }

        protected override CodingMonkeyContext CodingMonkeyContext { get; set; }

        protected override TimeSpan CacheEntryTimeoutValue { get; set; }

        protected override string CacheKeyPrefix { get; set; }

        protected override string AllCacheKey { get; set; }

        protected override MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        public TestRepository(IMemoryCache memoryCache, CodingMonkeyContext codingMonkeyContext)
        {
            this.MemoryCache = memoryCache;
            this.CodingMonkeyContext = codingMonkeyContext;

            this.CacheEntryTimeoutValue = TimeSpan.FromHours(24);
            this.CacheKeyPrefix = typeof(Test).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
            };
        }

        public List<Test> All(int exerciseId)
        {
            List<Test> tests = new List<Test>();

            string allCacheKeyScopedToExercise = this.GetAllCacheKeyScopedToExercise(exerciseId);

            if (!MemoryCache.TryGetValue(allCacheKeyScopedToExercise, out tests))
            {
                tests = CodingMonkeyContext.Tests
                                           .Include(x => x.TestInputs)
                                           .Include(x => x.TestOutput)
                                           .Include(x => x.Exercise)
                                           .Where(x => x.Exercise.ExerciseId == exerciseId).ToList();

                MemoryCache.Set(allCacheKeyScopedToExercise, tests, this.DefaultCacheEntryOptions);
            }

            return tests;
        }

        public Test GetById(int testId)
        {
            Test test = null;

            string testCacheKey = this.GetEntityCacheKey(testId);

            if (!MemoryCache.TryGetValue(testCacheKey, out test))
            {
                test = CodingMonkeyContext.Tests
                                          .Include(x => x.TestInputs)
                                          .Include(x => x.TestOutput)
                                          .Include(x => x.Exercise)
                                          .SingleOrDefault(e => e.TestId == testId);

                MemoryCache.Set(testCacheKey, test, this.DefaultCacheEntryOptions);
            }

            return test;
        }

        public Test Create(int exerciseId, Test entity)
        {
            try
            {
                Exercise relatedExercise = CodingMonkeyContext.Exercises
                                                              .SingleOrDefault(x => x.ExerciseId == exerciseId);

                if (relatedExercise == null) throw new Exception("Related exercise with ID: '{exerciseId}' could not be found");

                entity.RelateExerciseToTestInMemory(relatedExercise);
                CodingMonkeyContext.Tests.Add(entity);

                CodingMonkeyContext.SaveChanges();
                entity.RelateTestToTestIoInMemory();
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create test", ex);
            }

            string testCacheKey = this.GetEntityCacheKey(entity.TestId);
            MemoryCache.Set(testCacheKey, entity, this.DefaultCacheEntryOptions);

            MemoryCache.Remove(this.GetAllCacheKeyScopedToExercise(exerciseId));

            return entity;
        }

        public Test Update(int exerciseId, int testId, Test entity)
        {
            Test existingTest = null;

            try
            {
                existingTest = CodingMonkeyContext.Tests
                                                  .Include(x => x.TestInputs)
                                                  .Include(x => x.TestOutput)
                                                  .Include(x => x.Exercise)
                                                  .SingleOrDefault(e => e.TestId == testId && e.Exercise.ExerciseId == exerciseId);

                if (existingTest == null) throw new ArgumentException($"Existing test with id: '{testId}' could not be found.");

                //TODO: When EF7 Introduces AddOrUpdate - use that instead of code below
                // http://stackoverflow.com/questions/36208580/what-happened-to-addorupdate-in-ef-7

                CodingMonkeyContext.TestOutputs.Remove(existingTest.TestOutput);
                CodingMonkeyContext.TestInputs.RemoveRange(existingTest.TestInputs);

                CodingMonkeyContext.SaveChanges();

                existingTest.Description = entity.Description;
                existingTest.TestOutput = entity.TestOutput;
                existingTest.TestInputs = entity.TestInputs;

                // Ensure any test input / outputs are related in case new ones added
                existingTest.RelateTestToTestIoInMemory();

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update test", ex);
            }

            MemoryCache.Remove(this.GetEntityCacheKey(testId));
            MemoryCache.Set(this.GetEntityCacheKey(testId), existingTest);
            MemoryCache.Remove(this.GetAllCacheKeyScopedToExercise(exerciseId));

            return existingTest;
        }

        public void Delete(int testId)
        {
            Test test = CodingMonkeyContext.Tests.Include(t => t.TestInputs)
                                                 .Include(t => t.TestOutput)
                                                 .Include(t => t.Exercise)
                                                 .SingleOrDefault(t => t.TestId == testId);

            try
            {
                if (test == null) throw new ArgumentException($"Existing test with id: '{testId}' could not be found.");
                CodingMonkeyContext.Tests.Remove(test);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Test", ex);
            }

            MemoryCache.Remove(this.GetEntityCacheKey(testId));
            MemoryCache.Remove(this.GetAllCacheKeyScopedToExercise(test.Exercise.ExerciseId));
        }

        private string GetAllCacheKeyScopedToExercise(int exerciseId)
        {
            return $"{this.AllCacheKey}_{exerciseId}";
        }
    }
}
