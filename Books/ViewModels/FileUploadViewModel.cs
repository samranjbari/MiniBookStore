using Books.Data.Models;
using ServiceStack.ServiceHost;

namespace Books.ViewModels
{
    [Route("/fileUpload")]
    public class FileUploadViewModel : ModelBase
    {
        public string Filename { get; set; }

        public bool IsTemporary { get; set; }

        public string FileType { get; set; }
    }
}