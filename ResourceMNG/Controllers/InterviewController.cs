using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using ResourceEntity.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResourceMNG.Controllers
{
	public class InterviewController : Controller
	{
		private readonly ResourceMngContext context;

		public InterviewController(ResourceMngContext context)
		{
			this.context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RegisterInterview()
		{
			ViewBag.ApplicantId = context.Applicants
				.Select(v => new SelectListItem
				{
					Value = v.ApplicantId.ToString(),
					Text = v.ApplicantName
				})
				.ToList();
			return View();
		}
		[HttpPost]
		public IActionResult RegisterInterview(Interview interview, int title)
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
				var insertinterview = new Interview();
				insertinterview.InterviewSd = interview.InterviewSd;
				insertinterview.InterviewEd = interview.InterviewEd;
				insertinterview.InterviewDescription = interview.InterviewDescription;
				insertinterview.InterviewAddress = interview.InterviewAddress;
				insertinterview.ApplicantId = title;
				insertinterview.InterviewSchedule = interview.InterviewSchedule;
				context.Interviews.Add(insertinterview);
				context.SaveChanges();
				TempData["result"] = "Thêm thành công !!!";

				return RedirectToAction("ListInterview");
			}
			else
			{
				return View();
			}
		}

		[HttpGet]
		public JsonResult GetVacancy(int id)
		{
			var applicant = context.Applicants.Find(id);
			if (applicant == null)
			{
				return Json(new { result = "NO" });
			}
			var vacancy = context.Vacancies.Find(applicant.VacancyId);
			if (vacancy == null)
			{
				return Json(new { result = "NO" });
			}
			return Json(new { VacancyJobTitle = vacancy.VacancyJobTitle, VacancyHm = vacancy.VacancyHm });
		}

		public async Task<IActionResult> ListInterview(string title="", string ntd="", bool isWeek = false)
		{
			if (HttpContext.Session.GetString("nameUser") == null)
			{
				return RedirectToAction("Login", "User");
			}
			else
			{
				ViewBag.Message = "Your register list page.";
				var interviews = await context.Interviews
			   .Include(a => a.Applicant)
			   .Include(a=>a.Applicant.Vacancy)
			   .ToListAsync();

				var Filtervacancies = interviews;

				ViewBag.SearchList = interviews.Select(m => m.Applicant.Vacancy.VacancyJobTitle).Distinct().ToList();

				ViewBag.SearchListNTD = interviews.Select(m => m.Applicant.Vacancy.VacancyHm).Distinct().ToList();
				if (title == "All")
				{
					title = "";
				}

				if (ntd == "All")
				{
					ntd = "";
				}
				if (isWeek)
                {
					DateTime oneWeekAgo = DateTime.Now.AddDays(-7);
					Filtervacancies = interviews.Where(v => v.InterviewEd >= oneWeekAgo).ToList();
					ViewBag.Interviews = Filtervacancies;
					
				}
				else if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(ntd))
				{

					Filtervacancies = interviews.Where(v => v.Applicant.Vacancy.VacancyJobTitle.Contains(title) && v.Applicant.Vacancy.VacancyHm.Contains(ntd)).ToList();
					ViewBag.Interviews = Filtervacancies;
					ViewBag.SearchValueTitle = title;
					ViewBag.SearchValueNtd = ntd;
				}
				else
				{
				ViewBag.Interviews = interviews;
				}

				if (TempData["result"] != null)
				{
					ViewBag.Msg = TempData["result"];
				}

			}
			return View();

		}
		public async Task<IActionResult> Calendar()
		{
			//List<Interview> interviewData = new List<Interview>();
			var interviews = await context.Interviews
				   .Include(a => a.Applicant)
				   .ToListAsync();
			ViewBag.Interviews = interviews;
			return View();
		}
		public IActionResult ResultInterview(int id)
		{
			var interview = context.Interviews
				.Include(i => i.Applicant) // Đảm bảo load thông tin ứng cử viên
				.FirstOrDefault(i => i.InterviewId == id);

			if (interview == null)
			{
				return NotFound();
			}
			var applicant = context.Applicants.Where(a => a.ApplicantId == interview.ApplicantId).FirstOrDefault();
            var Listinterview = context.Interviews
                .Include(i => i.Applicant) // Đảm bảo load thông tin ứng cử viên
                .Where(i => i.ApplicantId == applicant.ApplicantId).ToList();
			ViewBag.Interviews = Listinterview;
            if (TempData["result"] != null)
			{
				ViewBag.Msg = TempData["result"];
			}
			return View(interview);
		}
		public IActionResult EditResult(int id)
		{
			var interview = context.Interviews
				.Include(i => i.Applicant) // Đảm bảo load thông tin ứng cử viên
				.FirstOrDefault(i => i.InterviewId == id);

			if (interview == null)
			{
				return NotFound();
			}
			ViewBag.Interviews = new List<Interview> { interview };
			
			return View(interview);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditResult(Interview interview)
		{
			if (ModelState.IsValid)
			{
				var existingInterview = context.Interviews.Find(interview.InterviewId);
				if (existingInterview != null)
				{
					existingInterview.InterviewResultOne = interview.InterviewResultOne;
					existingInterview.InterviewResultTwo = interview.InterviewResultTwo;

					context.Update(existingInterview);
					context.SaveChanges();

					TempData["result"] = "Chỉnh sửa thành công !!!";
					return RedirectToAction(nameof(ResultInterview), new { id = interview.InterviewId });
				}
			}
			return View(interview);
		}
	}
}
