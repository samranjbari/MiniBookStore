using System.Collections;
using System.Collections.Generic;
using Books.Data.API;
using Books.Data.Models;
using DapperExtensions;

namespace Books.Data.OrmLite
{
    public class BookRepository : Repository<BookModels>, IBookRepository
    {
        public object GetBooksWithCategory()
        {
            LookupRepository<CategoryModels> category = new LookupRepository<CategoryModels>();

            IEnumerable<CategoryModels> allCategories = category.All();

            List<object> list = new List<object>();

            foreach (var cat in allCategories)
            {
                var predicate = Predicates.Field<BookModels>(f => f.CategoryId, Operator.Eq, cat.Id);
                List<BookModels> books = this.Query(predicate) as List<BookModels>;

                if (books.Count > 0)
                {
                    var booksinCat = new
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        CategoryBooks = books
                    };

                    list.Add(booksinCat);
                }
                else
                {
                    var booksinCat = new
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        CategoryBooks = new List<BookModels>()
                    };

                    list.Add(booksinCat);
                }
            }

            return list;
        }
    }
}