namespace CodingMonkey.Models.Repositories
{
    using System.Collections.Generic;

    public interface IChildRepository<T>
    {
        List<T> All(int parentId);

        T GetById(int id, bool ignoreCache = false);

        T Create(int parentId, T entity);

        T Update(int parentId, int id, T entity);

        void Delete(int id);
    }
}
