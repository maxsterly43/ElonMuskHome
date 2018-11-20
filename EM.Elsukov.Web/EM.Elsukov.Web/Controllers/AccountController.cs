
using EM.Elsukov.DB.NHibernate;
using EM.Elsukov.DB.NHibernate.Interfaces;
using EM.Elsukov.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EM.Elsukov.Web.Controllers
{
    public class AccountController : Controller
    {
        IUserRepository users;
        public AccountController()
        {
            users = new NHUserRepository();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "ololol error");
                return View(loginModel);
            }
            else
            {
                var user = users.LoadByName(loginModel.Login);

                if (user == null || user.Password != loginModel.Password)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(loginModel);
                }

                FormsAuthentication.SetAuthCookie(user.Login, false);
            }
            return View(loginModel);
        }
    }
}