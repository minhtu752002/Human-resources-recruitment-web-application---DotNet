using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceEntity.Models;

namespace ResourceMNG.Controllers
{

    public class UserController : Controller
    {
        private ResourceMngContext context;

        public UserController(ResourceMngContext context)
        {
            this.context = context;
        }

        public IActionResult Login()
        {
            return View("LoginPro");
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var myuser = context.Users.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();

            if (myuser != null)
            {
                
                //HttpContext.Session.SetString("stringUser", myuser?.Email);
                HttpContext.Session.SetInt32("IDUser", myuser.Id);
                HttpContext.Session.SetString("nameUser", myuser?.Fullname);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Tài khoản hoặc mật khẩu không đúng !!!";
            }
            return View("LoginPro");
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("nameUser") != null)
            {
                HttpContext.Session.Remove("nameUser");
                HttpContext.Session.Remove("IDUser");
                return RedirectToAction("Login");
            }

            return View("LoginPro");
        }
    }
}
