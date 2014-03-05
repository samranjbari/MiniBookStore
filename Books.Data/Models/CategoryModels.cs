using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace Books.Data.Models
{
    [Route("/category")]
    [Alias("LU_Category")]
    public class CategoryModels : ModelBase
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int SortOrder { get; set; }
    }
}