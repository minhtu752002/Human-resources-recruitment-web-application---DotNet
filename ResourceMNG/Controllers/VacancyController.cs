using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using ResourceEntity.Models;
using System.Linq;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ResourceMNG.Controllers
{
    public class VacancyController : Controller

    {
        private readonly ResourceMngContext context;
        private readonly IHostingEnvironment hostingEnvironment;
        public VacancyController(IHostingEnvironment environment, ResourceMngContext context)
        {
            hostingEnvironment = environment;
            this.context = context;
        }

		public VacancyController(IWebHostEnvironment webHostEnvironment, ResourceMngContext context)
		{
			this.context = context;
		}

		public IActionResult RegisterVacancy()
        {
            return View();

        }
        [HttpPost]
        public IActionResult RegisterVacancy(NewVacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                if (vacancy.FileVacancyDetails != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(vacancy.FileVacancyDetails.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    vacancy.FileVacancyDetails.CopyTo(new FileStream(filePath, FileMode.Create));
                    vacancy.VacancyDetails = uniqueFileName;
                }

                var context = new ResourceMngContext();
                var insertvacancy = new Vacancy();
                insertvacancy.VacancyJobTitle = vacancy.VacancyJobTitle;
                insertvacancy.VacancyDetails = vacancy.VacancyDetails;
                insertvacancy.VacancyOd = vacancy.VacancyOd;
                insertvacancy.VacancyCd = vacancy.VacancyCd;
                insertvacancy.VacancyHm = vacancy.VacancyHm;
                insertvacancy.VacancyAmout = vacancy.VacancyAmout;
                context.Vacancies.Add(insertvacancy);

                context.SaveChanges();
                TempData["result"] = "Thêm thành công !!!";
                return RedirectToAction("ListVacancy");
            }
            else
            {
                return View();
            }
        }
        public IActionResult ListVacancy(string title = "",string ntd = "", bool expr = false)
        {

            if (HttpContext.Session.GetString("nameUser") == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                //string title = form["title"];
                ViewBag.Message = "Your vacancy list page.";
                //var context = new ResourceMngContext();
                var vacancies = context.Vacancies.ToList();
                var Filtervacancies = vacancies;
                ViewBag.SearchList = vacancies.Select(m=>m.VacancyJobTitle).Distinct().ToList();
               
                ViewBag.SearchListNTD = vacancies.Select(m=>m.VacancyHm).Distinct().ToList();
                if (expr)
                {
                    Filtervacancies = vacancies.Where(v => v.VacancyCd >= DateTime.Now).ToList();
                    ViewBag.Vacancies = Filtervacancies;
                }
                else
                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(ntd))
                {
                    if (title == "All")
                    {
                        title = "";
                    }

                    if (ntd == "All")
                    {
                        ntd = "";
                    }
                    Filtervacancies = vacancies.Where(v => v.VacancyJobTitle.Contains(title) && v.VacancyHm.Contains(ntd)).ToList();
                    ViewBag.Vacancies = Filtervacancies;
                    ViewBag.SearchValueTitle = title;
                    ViewBag.SearchValueNtd = ntd;

                }
                else
                {
                    ViewBag.Vacancies = vacancies;
                }

                if (TempData["result"] != null)
                {
                    ViewBag.Msg = TempData["result"];
                }
                if (TempData["resultERROR"] != null)
                {
                    ViewBag.MsgError = TempData["resultERROR"];
                }
            }
            return View();

        }
        public IActionResult Edit(int id)
        {
            //var context = new ResourceMngContext();
            var vacancy = context.Vacancies.Find(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return View(vacancy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewVacancy vacancy)
        {
            if (ModelState.IsValid)
            {
				if (vacancy.FileVacancyDetails != null)
				{
					var uniqueFileName = Common.GetUniqueFileName(vacancy.FileVacancyDetails.FileName);
					var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
					var filePath = Path.Combine(uploads, uniqueFileName);
					vacancy.FileVacancyDetails.CopyTo(new FileStream(filePath, FileMode.Create));
					vacancy.VacancyDetails = uniqueFileName;
				}
				//var context = new ResourceMngContext();
				context.Update(vacancy);

                context.SaveChanges();
                TempData["result"] = "Chỉnh sửa thành công !!!";
                return RedirectToAction(nameof(ListVacancy));
            }
            return View(vacancy);
        }

        public IActionResult DeleteVacacyModal(int id)
        {
            Vacancy vac = context?.Vacancies?.Where(p => p.VacancyId == id).FirstOrDefault();

            return PartialView("VacacyDeleteModal", vac);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteModalComfirm(Vacancy vacancy)
        {
            try
            {
				context.Vacancies.Remove(vacancy);
				context.SaveChanges();
				TempData["result"] = "Xóa thành công !!!";
			}
            catch (DbUpdateException ex)
            {
				TempData["resultERROR"] = "Không thể xóa vì có tham chiếu đến dữ liệu khác.";
            }
            //context.Vacancies.Remove(vacancy);
            //context.SaveChanges();
            //TempData["result"] = "Xóa thành công !!!";
            return RedirectToAction("ListVacancy");
        }


        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    //var context = new ResourceMngContext();
        //    var vacancy = context.Vacancies.Find(id);
        //    if (vacancy == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(vacancy);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var context = new ResourceMngContext();
        //    var vacancy = await context.Vacancies.FindAsync(id);
        //    if (vacancy == null)
        //    {
        //        return NotFound();
        //    }
        //    context.Vacancies.Remove(vacancy);
        //    await context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ListVacancy));
        //}
    }
}