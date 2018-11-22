
using EM.Elsukov.DB.NHibernate;
using EM.Elsukov.DB.NHibernate.Interfaces;
using EM.Elsukov.Web.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace EM.Elsukov.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IUserRepository users;
        public AccountController()
        {
            users = new NHUserRepository();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("AllNotes", "Notes");
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Некорректный ввод");
                return View(loginModel);
            }
            else
            {
                var user = users.LoadByLogin(loginModel.Login);

                if (user == null || user.Password != loginModel.Password)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(loginModel);
                }
                FormsAuthentication.SetAuthCookie(user.Login, false);
            }
            return RedirectToAction("AllNotes", "Notes");
        }
        [HttpGet]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}