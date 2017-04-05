namespace CodingMonkey.Models.Repositories
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        List<T> All();

        T GetById(int id, bool ignoreCache = false);

        T Create(T entity);

        T Update(int id, T entity);

        void Delete(int id);
    }
}
