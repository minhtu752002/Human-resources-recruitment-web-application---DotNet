using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using ResourceEntity.Models;
using ResourceMNG.BPO_HRMDataSetTableAdapters;
using System.Composition;
using System.Data;
using System.Drawing.Printing;
using static ResourceMNG.BPO_HRMDataSet;

namespace ResourceMNG.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ResourceMngContext context;

		public ReportController(IWebHostEnvironment webHostEnvironment, ResourceMngContext context)
        {
            this._webHostEnvironment = webHostEnvironment;
			this.context = context;
		}
        public IActionResult Index()
        {
            return View();
        }
		#region  Common
		public IActionResult GenerateReportExcel<T>(IEnumerable<T> dataSource, string reportTitle, string reportPath)
        {
            string mintype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{reportPath}";
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            parameter.Add("title", reportTitle);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dataSource);
            var result = localReport.Execute(RenderType.ExcelOpenXml, extension, parameter, mintype);
            return File(result.MainStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{reportTitle}.xlsx");

        }
		public IActionResult GenerateReportImage<T>(IEnumerable<T> dataSource, string reportTitle, string reportPath)
		{
			string mintype = "";
			int extension = 1;
			var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{reportPath}";
			Dictionary<string, string> parameter = new Dictionary<string, string>();
			parameter.Add("title", reportTitle);
			//var project = context.Projects.Where(p => p.ProjectActive == "Y").ToList();
			//BPO_HRMDataSet.ProjectRow row = new BPO_HRMDataSet.ProjectRow();
			//row.ProjectID.ToString();
			LocalReport localReport = new LocalReport(path);
			localReport.AddDataSource("DataSet1", dataSource);
			var result = localReport.Execute(RenderType.Pdf, extension, parameter, mintype);
			return File(result.MainStream, "application/pdf", $"{reportTitle}.pdf");
		}
		#endregion
        //tạm dừng dự án
		public IActionResult PrintProject(string isActive = "")
        {
           
            BPO_HRMDataSet.ProjectDataTable a = new BPO_HRMDataSet.ProjectDataTable();
            ProjectTableAdapter adapter = new ProjectTableAdapter();
            a = adapter.GetDataByActive(isActive);
			string reportTitle = "Báo cáo dự án";
            string reportPath = "Project\\Report.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);
        }
        public IActionResult PrintApplicant(string title = "")
        {
            //var applicant = context.Applicants.Where(p => p.Vacancy.VacancyJobTitle == title).ToList();

            BPO_HRMDataSet.ApplicantDataTable a = new ApplicantDataTable();
            //ApplicantTable_VacancyAdapter adapter = new ApplicantTable_VacancyAdapter();
            ApplicantTableAdapter app  = new ApplicantTableAdapter();
            a = app.GetDataByTitleJob(title);
            string reportTitle = "Báo cáo ứng cử viên";
            string reportPath = "Applicant\\Report.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);
        }
        public IActionResult PrintApplicantAll()
        {
            //var applicant = context.Applicants.Where(p => p.Vacancy.VacancyJobTitle == title).ToList();

            BPO_HRMDataSet.ApplicantDataTable a = new ApplicantDataTable();
            //ApplicantTable_VacancyAdapter adapter = new ApplicantTable_VacancyAdapter();
            ApplicantTableAdapter app  = new ApplicantTableAdapter();
            a = app.GetDataApplicantAll();
            string reportTitle = "Báo cáo ứng cử viên";
            string reportPath = "Applicant\\Report.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);
        }
       
        public  IActionResult PrintNhansu()
        {
            //var employees = context.Employees.ToList();
            BPO_HRMDataSet.EmployeeDataTable a = new BPO_HRMDataSet.EmployeeDataTable();
            EmployeeTableAdapter emp = new EmployeeTableAdapter();
            a = emp.GetData();
            string reportTitle = "Báo cáo nhân sự";
            string reportPath = "Employee\\Report.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);
        }
        public IActionResult PrintVacancy(string title ="", string ntd = "")
        {
            BPO_HRMDataSet.VacancyDataTable a = new VacancyDataTable();
            VacancyTableAdapter adapter = new VacancyTableAdapter();
             a =  adapter.GetDataByTitleAndNtd(title, ntd);
            string reportTitle = "Báo cáo tuyển dụng";
            string reportPath = "Vacancy\\ReportVacancy.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);

        }  
        public IActionResult PrintInterview(string title ="", string ntd = "")
        {
            BPO_HRMDataSet.InterviewDataTable a = new InterviewDataTable();
            InterviewTableAdapter adapter = new InterviewTableAdapter();
             a =  adapter.GetDataByJobAndHm(title, ntd);
            string reportTitle = "Báo cáo tuyển dụng";
            string reportPath = "Interview\\Report.rdlc";
            return GenerateReportExcel(a, reportTitle, reportPath);

        }

        public IActionResult ImageVacancy()
        {
            //var projects = context.Projects.Where(p => p.ProjectActive == "Y").ToList();

            BPO_HRMDataSet.VacancyCounterDataTable a = new BPO_HRMDataSet.VacancyCounterDataTable();
            VacancyCounterTableAdapter adapter = new VacancyCounterTableAdapter();
            a = adapter.GetData();
            string reportTitle = "Phân bổ nhân sự cho từng dự án";
            string reportPath = "Vacancy\\Report.rdlc";
            return GenerateReportImage(a, reportTitle, reportPath);
        }

    }
}
