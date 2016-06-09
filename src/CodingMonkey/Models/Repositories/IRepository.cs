namespace CodingMonkey.Models.Repositories
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        List<T> All();

        T GetById(int id);

        void Create(T entity);

        void Update(int id, T entity);

        void Delete(int id);
    }
}
