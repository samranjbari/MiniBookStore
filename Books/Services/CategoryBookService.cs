using ServiceStack.ServiceInterface;
using Books.ViewModels;
using Books.Data.API;
using Books.Data.Models;

namespace Books.Services
{
    public class CategoryBookService : Service
    {
        public IBookRepository BookRepository { get; set; }
        public ILookupRepository<CategoryModels> CategoryLookup { get; set; }

        public object Get(CategoryViewModel request)
        {
            var response = BookRepository.GetBooksWithCategory();
            return response;
        }

        [Authenticate]
        public void Delete(CategoryViewModel request)
        {
            CategoryLookup.Delete(request.Id);
        }
    }
}