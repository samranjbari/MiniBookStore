using System.Web.Mvc;
using Books.Parser;
using ServiceStack.OrmLite;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Mvc;
using Books.Data.Models;

namespace Books.Controllers
{
    public class BookController : ServiceStackController
    {
        public ActionResult CreateBook()
        {
            var authSession = this.SessionAs<AuthUserSession>();
            if (authSession == null || !authSession.IsAuthenticated)
            {
                Response.Redirect("/account/logon");
            }

            return View();
        }

        public ActionResult Administrative()
        {
            var authSession = this.SessionAs<AuthUserSession>();
            if (authSession == null || !authSession.IsAuthenticated)
            {
                Response.Redirect("/account/logon");
            }

            if (!authSession.HasRole(UserRoles.Admin.ToString()))
            {
                Response.Redirect("/book/createbook");
            }

            return View();
        }
    }
}