using System.Collections.Generic;

namespace Books.Data.API
{
    public interface IRepository<T>
    {
        T Find(long id);
        int Insert(T entity);
        IEnumerable<T> Insert(IEnumerable<T> entities);
        bool Update(T entity);
        void Delete(int id);
        IEnumerable<T> All();
    }
}