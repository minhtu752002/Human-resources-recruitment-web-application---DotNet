using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using ResourceEntity.Models;

namespace ResourceMNG.Controllers
{
    public class AccountController : Controller
    {
        private readonly ResourceMngContext context;

        public AccountController(ResourceMngContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("nameUser") == null)
            {
                return RedirectToAction("Login", "User");
            }else
            {
            var account = context.Users.ToList();
            ViewBag.Account = account;
                if (TempData["result"] != null)
                {
                    ViewBag.Msg = TempData["result"];
                }
            }

            return View();
        }
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            if (ModelState.IsValid)
            {
                var insertitem = new User();
                insertitem.Fullname = user.Fullname;
                insertitem.Username = user.Username;
                insertitem.Password = user.Password;
                insertitem.Email = user.Email;
                insertitem.Address = user.Address;
                insertitem.Phone = user.Phone;
                context.Users.Add(insertitem);
                context.SaveChanges();
                TempData["result"] = "Thêm thành công !!!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult CapQuyenModal(int id)
        {
            var user = context.Users.Find(id);
            var getRoleDesc = context.RoleDescs.OrderBy(m=>m.Stt).ToList();
            var rolesUser = context.RoleDescs
            .Join(context.PhanQuyens,
                rd => rd.Id,
                pq => pq.RoleId,
                (rd, pq) => new { RoleDesc = rd, PhanQuyen = pq })
            .Where(joined => joined.PhanQuyen.UserId == id)
            .Select(joined => joined.RoleDesc)
            .ToList();
            if (user == null || getRoleDesc ==null || rolesUser == null)
            {
                return NotFound();
            }
   

            ViewBag.RoleDesc = getRoleDesc;
            ViewBag.RolesUser = rolesUser;
            return View("CapQuyen", user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CapQuyenUser(User user,List<int> selectedRoles)
        {
            
            var existingPhanQuyens = context.PhanQuyens.Where(pq => pq.UserId == user.Id);
            context.PhanQuyens.RemoveRange(existingPhanQuyens);
            context.SaveChanges();

            List<PhanQuyen> newPhanQuyens = new List<PhanQuyen>();
            foreach (int roleId in selectedRoles)
            {
                PhanQuyen newPhanQuyen = new PhanQuyen
                {
                    UserId = user.Id,
                    RoleId = roleId
                };

                newPhanQuyens.Add(newPhanQuyen);
            }

            context.PhanQuyens.AddRange(newPhanQuyens);
            context.SaveChanges();
            TempData["result"] = "Cấp quyền thành công !!!";
            return RedirectToAction("Index");
            
         
        }
     
        
        public async Task<IActionResult> EditAccount(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult EditAccount(User user)
        {
            if (ModelState.IsValid)
            {
               
                //var context = new ResourceMngContext();
                context.Update(user);
                TempData["result"] = "Chỉnh sửa thành công !!!";

                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
       
        public async Task<IActionResult> ChangeAccountUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeAccountUser(User user)
        {
            if (ModelState.IsValid)
            {

                //var context = new ResourceMngContext();
                context.Update(user);

                context.SaveChanges();
                ViewBag.MsgAcc = "Chỉnh sửa thành công !!!";
                return View("ChangeAccountUser");
            }
            return View(user);
        }

    }
}
