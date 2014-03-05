using ServiceStack.ServiceHost;

namespace Books.ViewModels
{
    [Route("/featureBook")]
    public class FeaturedBooksViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}