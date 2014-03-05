using ServiceStack.ServiceInterface;
using Books.Data.API;
using Books.Data.Models;

namespace Books.Services
{
    public class BookPropertyService : Service
    {
        public IBookPropertyRepository BookPropertyRepository { get; set; }

        public object Get(BookPropertyModels request)
        {
            return null;
        }
    }
}