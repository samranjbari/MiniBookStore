using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.FluentValidation;

namespace Books.Data.Models
{
    [Route("/bookproperty")]
    [Alias("BookProperties")]
    public class BookPropertyModels : ModelBase
    {
        [AutoIncrement]
        [PrimaryKey]
        public long Id { get; set; }

        [References(typeof(BookModels))]
        [Index]
        [Required]
        public long BookId { get; set; }

        public int? Rating { get; set; }
    }

    public class BookPropertyValidator : AbstractValidator<BookPropertyModels>
    {
        public BookPropertyValidator()
        {
            RuleFor(e => e.Rating).InclusiveBetween(0, 5);
        }
    }
}