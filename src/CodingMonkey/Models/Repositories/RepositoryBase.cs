namespace CodingMonkey.Models.Repositories
{
    using System;

    using Microsoft.Extensions.Caching.Memory;

    public abstract class RepositoryBase
    {
        protected abstract IMemoryCache MemoryCache { get; set; }
        protected abstract CodingMonkeyContext CodingMonkeyContext { get; set; }
        protected abstract TimeSpan CacheEntryTimeoutValue { get; set; }
        protected abstract string CacheKeyPrefix { get; set; }
        protected abstract string AllCacheKey { get; set; }
        protected abstract MemoryCacheEntryOptions DefaultCacheEntryOptions { get; set; }

        protected virtual string GetEntityCacheKey(int id)
        {
            return $"{this.CacheKeyPrefix}_{id}";
        }
    }
}
