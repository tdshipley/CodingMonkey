namespace CodingMonkey.Models.Repositories
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Caching.Memory;

    public class TestRepository : RepositoryBase, IRepository<Test>
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

            this.CacheEntryTimeoutValue = TimeSpan.FromHours(2);
            this.CacheKeyPrefix = typeof(ExerciseCategory).Name.ToLower();
            this.AllCacheKey = $"{CacheKeyPrefix}_all";
            this.DefaultCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = this.CacheEntryTimeoutValue
            };
        }

        public List<Test> All()
        {
            throw new NotImplementedException();
        }

        public Test GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(Test entity)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Test entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
