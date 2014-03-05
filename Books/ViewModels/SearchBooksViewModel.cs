using Books.Data.Models;
using ServiceStack.ServiceHost;

namespace Books.ViewModels
{
    [Route("/searchbook")]
    public class SearchBooksViewModel : ModelBase
    {
        public string Author { get; set; }
        public string Popular { get; set; }
    }
}