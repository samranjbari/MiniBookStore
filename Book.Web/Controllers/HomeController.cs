using System.Data;
using System.Web.Mvc;
using Books.Data.API;
using ServiceStack.Text;

namespace Books.Controllers
{
    public class HomeController : Controller
    {
        public IBookRepository BookRepository { get; set; }

        public ActionResult Index()
        {
            ViewBag.Title = "Cocaine Books - Gain Exposure to our Thousands of Readers";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Cocaine Books - Gain Exposure to our Thousands of Readers";

            return View();
        }

        public ActionResult Detail(string id)
        {
            var book = BookRepository.Find(id.To<long>());
            if (book != null)
            {
                ViewBag.Title = "Checkout {0} ".Fmt(book.Title);
            }

            return View();
        }

        public ActionResult FindBook()
        {
            ViewBag.Title = "Cocaine Books - Gain Exposure to our Thousands of Readers";

            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Teaser()
        {
            return View();
        }
    }
}
