using System.Web.Mvc;
using System.Web.Security;
using ServiceStack.Common;

namespace Books.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Reset()
        {
            //string url = Request.QueryString.ToString();

            //CryptUtils.Length = RsaKeyLengths.Bit1024;
            //CryptUtils.KeyPair = CryptUtils.CreatePublicAndPrivateKeyPair();

            //string decrypted = url.Decrypt();
            
            return View();
        }
    }
}
