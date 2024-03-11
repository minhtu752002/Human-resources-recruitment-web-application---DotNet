using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResourceEntity.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ResourceMNG.Controllers
{
	public class ApplicantController : Controller
	{
		private readonly ResourceMngContext context;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        [Obsolete]
        public ApplicantController(IHostingEnvironment environment, ResourceMngContext context)
		{
			hostingEnvironment = environment;
			this.context = context;
		}
		public IActionResult RegisterApplicant()
		{
            ViewBag.VacancyId = context.Vacancies
                .Select(v => new SelectListItem
                {
                    Value = v.VacancyId.ToString(),
                    Text = v.VacancyJobTitle
                })
                .ToList();
            return View();
        }
		[HttpPost]
		public IActionResult RegisterApplicant(NewApplicant applicant)
		{
			ViewBag.VacancyId = context.Vacancies
				.Select(v => new SelectListItem
				{
					Value = v.VacancyId.ToString(),
					Text = v.VacancyJobTitle
				})
				.ToList();

			if (ModelState.IsValid)
			{
				if (applicant.FileApplicantPicture != null)
				{
					var uniqueFileName = Common.GetUniqueFileName(applicant.FileApplicantPicture.FileName);
					var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
					var filePath = Path.Combine(uploads, uniqueFileName);
					applicant.FileApplicantPicture.CopyTo(new FileStream(filePath, FileMode.Create));
					applicant.ApplicantPicture = uniqueFileName;
				}
				if (applicant.FileApplicantCv != null)
				{
					var uniqueFileName = Common.GetUniqueFileName(applicant.FileApplicantCv.FileName);
					var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
					var filePath = Path.Combine(uploads, uniqueFileName);
					applicant.FileApplicantCv.CopyTo(new FileStream(filePath, FileMode.Create));
					applicant.ApplicantCv = uniqueFileName;
				}

				//var context = new ResourceMngContext();
				var insertapplicant = new Applicant();
				insertapplicant.ApplicantName = applicant.ApplicantName;
				insertapplicant.ApplicantDob = applicant.ApplicantDob;
				insertapplicant.ApplicantPob = applicant.ApplicantPob;
				insertapplicant.Gender = applicant.Gender;
				insertapplicant.ApplicantAddress = applicant.ApplicantAddress;
				insertapplicant.ApplicantPicture = applicant.ApplicantPicture;
				insertapplicant.ApplicantEmail = applicant.ApplicantEmail;
				insertapplicant.ApplicantPhone = applicant.ApplicantPhone;
				insertapplicant.ApplicantCv = applicant.ApplicantCv;
				insertapplicant.VacancyId= applicant.VacancyId;	
				context.Applicants.Add(insertapplicant);
				context.SaveChanges();
                TempData["result"] = "Thêm thành công !!!";

                return RedirectToAction("ListApplicant");
			}
			else
			{
				return View();
			}
		}
		public async Task<IActionResult> ListApplicant(string title = "")
		{
            if (HttpContext.Session.GetString("nameUser") == null)
            {
                return RedirectToAction("Login", "User");
            }else
            {
            var applicants = await context.Applicants
           .Include(a => a.Vacancy)
           .ToListAsync();
            ViewBag.Applicants = applicants;
		    ViewBag.SearchList = applicants.Select(m => m.Vacancy.VacancyJobTitle).Distinct().ToList();

				if (!string.IsNullOrEmpty(title) && title != "All")
				{

					applicants = applicants.Where(v => v.Vacancy.VacancyJobTitle.Contains(title)).ToList();
					ViewBag.Applicants = applicants;
                    ViewBag.SearchValue = title;
				}
				else
				{
					ViewBag.Applicants = applicants;
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
            ViewBag.VacancyId = context.Vacancies
                .Select(v => new SelectListItem
                {
                    Value = v.VacancyId.ToString(),
                    Text = v.VacancyJobTitle
                })
                .ToList();
            //var context = new ResourceMngContext();
            var applicant = context.Applicants.Find(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewApplicant applicant)
        {
           
            if (ModelState.IsValid)
            {
                if (applicant.FileApplicantPicture != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(applicant.FileApplicantPicture.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    applicant.FileApplicantPicture.CopyTo(new FileStream(filePath, FileMode.Create));
                    applicant.ApplicantPicture = uniqueFileName;
                }
                if (applicant.FileApplicantCv != null)
                {
                    var uniqueFileName = Common.GetUniqueFileName(applicant.FileApplicantCv.FileName);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    applicant.FileApplicantCv.CopyTo(new FileStream(filePath, FileMode.Create));
                    applicant.ApplicantCv = uniqueFileName;
                }
               
                //var context = new ResourceMngContext();
                context.Update(applicant);
                context.SaveChanges();
                TempData["result"] = "Chỉnh sửa thành công !!!";

                return RedirectToAction(nameof(ListApplicant));
            }
            return View(applicant);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ChangeToEmployee(NewApplicant applicant)
		{

			if (ModelState.IsValid)
			{
				var insertApplicantToEmp = new Employee();
				insertApplicantToEmp.EmployeeName = applicant.ApplicantName;
				insertApplicantToEmp.EmployeeGender = applicant.Gender;
				insertApplicantToEmp.EmployeeAddress = applicant.ApplicantAddress;
				insertApplicantToEmp.EmployeeAvatar = applicant.ApplicantPicture;
				insertApplicantToEmp.EmployeeCv = applicant.ApplicantCv;
				insertApplicantToEmp.EmployeeEmail = applicant.ApplicantEmail;
				insertApplicantToEmp.EmployeePhone = applicant.ApplicantPhone;
				context.Employees.Add(insertApplicantToEmp);
				context.SaveChanges();
				TempData["result"] = "Thêm thành công !!!";

				return RedirectToAction(nameof(ListApplicant));
			}
			return View(applicant);
		}

		public IActionResult DeleteApplicantModal(int id)
        {
            Applicant applicant = context?.Applicants?.Where(p => p.ApplicantId == id).FirstOrDefault();

            return PartialView("DeleteApplicantModal", applicant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteModalComfirm(Applicant applicant)
        {
            context.Applicants.Remove(applicant);
            context.SaveChanges();
            TempData["result"] = "Xóa thành công !!!";
            return RedirectToAction("ListApplicant");
        }
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    //var context = new ResourceMngContext();
        //    var applicant = context.Applicants.Find(id);
        //    if (applicant == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(applicant);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var context = new ResourceMngContext();
        //    var applicant = await context.Applicants.FindAsync(id);
        //    if (applicant == null)
        //    {
        //        return NotFound();
        //    }
        //    context.Applicants.Remove(applicant);
        //    await context.SaveChangesAsync();
        //    return RedirectToAction(nameof(ListApplicant));
        //}
    }
}
