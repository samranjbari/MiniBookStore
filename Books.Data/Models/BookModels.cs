using System.ComponentModel.DataAnnotations;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.FluentValidation;

namespace Books.Data.Models
{
    [Route("/book")]
    [Alias("Books")]
    public class BookModels : ModelBase, IReturn<BookResponse>
    {
        [AutoIncrement]
        public long Id { get; set; }

        [StringLength(150)]
        public string AmazonUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string CoverUrl { get; set; }

        [Required]
        [Index(Unique = true)]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        [Required]
        [DecimalLength(2)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(1500)]
        public string Description { get; set; }

        [References(typeof(CategoryModels))]
        public long CategoryId { get; set; }

        [StringLength(250)]
        public string GumroadUrl { get; set; }

        [StringLength(250)]
        public string BookUrl { get; set; }

        public bool IsFeature { get; set; }

        public int ReadCount { get; set; }

        [StringLength(250)]
        [DapperIgnore]
        public string CategoryText { get; set; }

        public int VoteRating { get; set; }

        public int UserId { get; set; }

        [DapperIgnore]
        public bool InsertMode { get; set; }

        [DapperIgnore]
        public string ShortDescription
        {
            get
            {
                if (this.Description.Length > 200)
                {
                    return this.Description.Substring(0, 200);
                }

                return this.Description;
            }
        }
    }

    public class BookResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class BookValidator : AbstractValidator<BookModels>
    {
        // ref. look at fluentvalidation.codeplex.com

        public BookValidator()
        {
            RuleFor(e => e.Price).GreaterThanOrEqualTo(0);
            RuleFor(e => e.Author).NotEmpty();
            RuleFor(e => e.CategoryId).NotEmpty().When(a => a.CategoryText == string.Empty);
            RuleFor(e => e.CoverUrl).NotEmpty().WithMessage("Upload a Cover for your book");
            //RuleFor(e => e.GumroadUrl).NotEmpty();
            RuleFor(e => e.Title).NotEmpty();
            RuleFor(e => e.Description).NotEmpty();
        }
    }
}