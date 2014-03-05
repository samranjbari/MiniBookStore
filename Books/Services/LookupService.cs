using Books.Data.Models;
using ServiceStack.ServiceInterface;
using Books.Data.API;

namespace Books.Services
{
    public class LookupService : Service
    {
        public ILookupRepository<CategoryModels> LookupRepository { get; set; }

        public object Get(CategoryModels request)
        {
            var response = LookupRepository.All();
            return response;
        }
    }
}