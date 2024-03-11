using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceEntity.Models;
using ResourceMNG.BPO_HRMDataSetTableAdapters;
using ResourceMNG.Models;
using System.Diagnostics;
using System.Linq;

namespace ResourceMNG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IDUser") == null)
            {
                return RedirectToAction("Login", "User");
            }


            var context = new ResourceMngContext();
            var chartData = new ChartViewModel();

            // Lấy dữ liệu số lượng người ứng tuyển và số lượng người đậu phỏng vấn
            chartData.Vacancies = context.Vacancies.Select(v => v.VacancyJobTitle + "-" + v.VacancyAmout).ToList();
            var vacancies = context.Vacancies.Include(v => v.Applicants).ToList();
            var data = new List<int>();
            var interviewcount = new List<int>();
            foreach (var item in vacancies.Where(a => a.VacancyAmout > 0))
            {
                var count = context.Applicants.Count(a => a.VacancyId == item.VacancyId);
                data.Add(count);

                var count2 = context.Interviews.Where(a => a.Applicant.Vacancy.VacancyId == item.VacancyId && a.InterviewResultOne == "Y").Count();
                interviewcount.Add(count2);

            }
            chartData.Applicants= data;
            chartData.InterviewResults = interviewcount;

            //danh sách nam nữ
            var employees = context.Employees.ToList();
            int maleCount = employees.Count(e => e.EmployeeGender == "Nam");
            int femaleCount = employees.Count(e => e.EmployeeGender == "Nữ");
            ViewBag.GenderMale = maleCount;
            ViewBag.GenderFeMale = femaleCount;
            //Lấy thời gian theo tuần
			DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var interviewsByWeek = context.Interviews.Include(i => i.Applicant).Where(i => i.InterviewSd >= startOfWeek && i.InterviewSd <= endOfWeek).ToList();

            var interviews = context.Interviews.Include(i => i.Applicant).ToList();

            var countEmpDepart = employees
	                        .GroupBy(e => e.EmployeeDepartment)
                            .Distinct()
	                        .Select(group => new
	                        {
		                        Department = group.Key, 
		                        Count = group.Count() 
	                        })
	                        .ToList();
			var departmentLabels = countEmpDepart.Select(d => d.Department).ToList();
			var employeeCounts = countEmpDepart.Select(d => d.Count).ToList();
			ViewBag.departmentLabels = departmentLabels;
			ViewBag.employeeCounts = employeeCounts;

			ViewBag.Interviews = interviews;
            ViewBag.InterviewsByWeek = interviewsByWeek;
			return View(chartData);
        }
    }
}

