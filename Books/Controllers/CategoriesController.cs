using System.Web.Mvc;

namespace Books.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult Index(string id)
        {
            return View();
        }
    }
}
