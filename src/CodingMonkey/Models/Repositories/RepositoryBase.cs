namespace CodingMonkey.Models.Repositories
{
    using System;

    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Generic;

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

        protected virtual void CreateEntityInCacheById<T>(int id, T entity)
        {
            string entityCacheKey = this.GetEntityCacheKey(id);

            //1. Add entity to cache
            MemoryCache.Set(entityCacheKey, entity, this.DefaultCacheEntryOptions);

            //2. Remove list from cache
            MemoryCache.Remove(this.AllCacheKey);
        }

        protected virtual List<T> TryGetAllInCache<T>(out bool success)
        {
            List<T> entites = null;

            //1. Try get value
            success = MemoryCache.TryGetValue(this.AllCacheKey, out entites);

            //2. Return value
            return entites;
        }

        protected virtual T TryGetEntityInCacheById<T>(int id, out bool success)
        {
            //1. Get cache key
            T entity = default(T);
            string entityCacheKey = this.GetEntityCacheKey(id);

            //2. Try get value
            success = MemoryCache.TryGetValue(entityCacheKey, out entity);

            //3. Return value or default
            return entity;
        }

        protected virtual void UpdateEntityInCacheById<T>(int id, T entity)
        {
            string entityCacheKey = this.GetEntityCacheKey(id);

            //1. Remove entity from cache
            MemoryCache.Remove(entityCacheKey);

            //2. Remove all list from cache
            MemoryCache.Remove(this.AllCacheKey);

            //3. Add new entity to cache
            MemoryCache.Set(entityCacheKey, entity, this.DefaultCacheEntryOptions);
        }

        protected virtual void DeleteEntityInCacheById(int id)
        {
            //1. Remove entity from cache
            string entityCacheKey = this.GetEntityCacheKey(id);
            MemoryCache.Remove(entityCacheKey);

            //2. Remove all list from cache
            MemoryCache.Remove(this.AllCacheKey);
        }
    }
}
