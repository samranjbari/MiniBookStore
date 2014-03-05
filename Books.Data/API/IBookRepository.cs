using Books.Data.Models;
using ServiceStack.OrmLite;

namespace Books.Data.API
{
    public interface IBookRepository : IRepository<BookModels>
    {
        object GetBooksWithCategory();
    }
}