using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using ResourceEntity.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ResourceMNG.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ResourceMngContext context;


        private readonly IHostingEnvironment hostingEnvironment;
        public EmployeeController(IHostingEnvironment environment, ResourceMngContext context)
        {
            hostingEnvironment = environment;
            this.context = context;
        }
        public IActionResult RegisterEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterEmployee(NewEmployee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.FileEmployeeAvatar != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(employee.FileEmployeeAvatar.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    employee.FileEmployeeAvatar.CopyTo(new FileStream(filePath, FileMode.Create));
                    employee.EmployeeAvatar = uniqueFileName;
                }
                if (employee.FileEmployeeCv != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(employee.FileEmployeeCv.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    employee.FileEmployeeCv.CopyTo(new FileStream(filePath, FileMode.Create));
                    employee.EmployeeCv = uniqueFileName;
                }

                //var context = new ResourceMngContext();
                var insertemployee = new Employee();
                insertemployee.EmployeeName = employee.EmployeeName;
                insertemployee.EmployeeAddress = employee.EmployeeAddress;
                insertemployee.EmployeeEmail = employee.EmployeeEmail;
                insertemployee.EmployeePhone = employee.EmployeePhone;
                insertemployee.EmployeeCv = employee.EmployeeCv;
                insertemployee.EmployeeAvatar = employee.EmployeeAvatar;
                insertemployee.EmployeeDepartment = employee.EmployeeDepartment.Trim();
                context.Employees.Add(insertemployee);
                TempData["result"] = "Thêm thành công !!!";
                context.SaveChanges();
                return RedirectToAction("ListEmployee");
            }
            else
            {
                return View();
            }
        }
        public IActionResult ListEmployee(string pb="")
        {
            if (HttpContext.Session.GetString("nameUser") == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                    ViewBag.Message = "Your register list page.";
                    //var context = new ResourceMngContext();
                    var employees = context.Employees.Where(e => e.EmployeeDelete == null).ToList();
                    ViewBag.Employees = employees;
				    ViewBag.SearchList = employees.Select(m => m.EmployeeDepartment).Distinct().ToList();

				if (!string.IsNullOrEmpty(pb) && pb != "All")
				{

					employees = employees.Where(v => v.EmployeeDepartment.Equals(pb)).ToList();
					ViewBag.Employees = employees;
                    ViewBag.SearchValue = pb;
				}
				else
				{
					ViewBag.Employees = employees;
				}
				if (TempData["result"] != null)
                    {
                        ViewBag.Msg = TempData["result"];
                    }
                
            }
            return View();

        }

        public IActionResult Edit(int id)
        {
            //var context = new ResourceMngContext();
            var emp = context.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewEmployee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.FileEmployeeCv != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(employee.FileEmployeeCv.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    employee.FileEmployeeCv.CopyTo(new FileStream(filePath, FileMode.Create));
                    employee.EmployeeCv = uniqueFileName;
                }

                if (employee.FileEmployeeAvatar != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(employee.FileEmployeeAvatar.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    employee.FileEmployeeAvatar.CopyTo(new FileStream(filePath, FileMode.Create));
                    employee.EmployeeAvatar = uniqueFileName;
                }
                //var context = new ResourceMngContext();
                context.Update(employee);
                TempData["result"] = "Chỉnh sửa thành công !!!";

                context.SaveChanges();
                return RedirectToAction(nameof(ListEmployee));
            }
            return View(employee);
        }
        public IActionResult DeleteEmpModal(int id)
        {
            Employee emp = context?.Employees?.Where(p => p.EmployeeId == id).FirstOrDefault();
            if (emp == null)
            {
                Console.WriteLine("Lỗi" + emp);
            }
            return PartialView("EmployeeModalDelete", emp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteModalComfirm(Employee employee)
        {
            var EmpToDelete = context.Employees.FirstOrDefault(p => p.EmployeeId == employee.EmployeeId);
            if (EmpToDelete != null)
            {
                EmpToDelete.EmployeeDelete = "Y";
                await context.SaveChangesAsync();
                TempData["result"] = "Xóa thành công";
            }
            ////context.Employees.Remove(employee);
            //context.SaveChanges();
            //TempData["result"] = "Xóa thành công !!!";

            return RedirectToAction("ListEmployee");
        }

        //Không dùng
        //[HttpGet]
        //public IActionResult DeleteEmp(int id)
        //{
        //    //var context = new ResourceMngContext();
        //    var Employees = context.Employees.Find(id);
        //    if (Employees == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Employees);
        //}

        //[HttpPost, ActionName("DeleteEmp")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var context = new ResourceMngContext();
        //    var emp = await context.Employees.FindAsync(id);
        //    if (emp == null)
        //    {
        //        return NotFound();
        //    }
        //    context.Employees.Remove(emp);
        //    await context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ListEmployee));
        //}
    }
}
