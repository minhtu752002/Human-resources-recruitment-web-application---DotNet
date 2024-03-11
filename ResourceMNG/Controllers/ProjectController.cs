using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using ResourceEntity.Models;
using Project = ResourceEntity.Models.Project;

namespace ResourceMNG.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ResourceMngContext context;

        public ProjectController(ResourceMngContext context)
        {
            this.context = context;
        }
        public IActionResult RegisterProject()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterProjectModal()
        {
            Project proj = new Project();

            return PartialView("CreateModal", proj);
        }

        [HttpPost]
        public IActionResult RegisterProject(Project project)
        {
            if (ModelState.IsValid)
            {
                var context = new ResourceMngContext();
                var insertitem = new Project();
                insertitem.ProjectName = project.ProjectName;
                insertitem.ProjectDescription = project.ProjectDescription;
                insertitem.ProjectClient = project.ProjectClient;
                insertitem.ProjectActive = project.ProjectActive;
                context.Projects.Add(insertitem);
                context.SaveChanges();
                TempData["result"] = "Thêm thành công !!!";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("nameUser") == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                    ViewBag.Message = "Your register list page.";
                    //var context = new ResourceMngContext();
                    var projects = context.Projects.Where(p=>p.ProjectDelete == null).ToList();
                    ViewBag.Projects = projects;
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
            var project = context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                //var context = new ResourceMngContext();
                context.Update(project);
                context.SaveChanges();
                TempData["result"] = "Chỉnh sửa thành công !!!";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public IActionResult DeleteProjectModal(int id)
        {
            Project project = context?.Projects?.Where(p => p.ProjectId == id).FirstOrDefault();

            return PartialView("DeleteModal", project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteModalComfirm(Project project)
        {
            var projectToDelete = context.Projects.FirstOrDefault(p => p.ProjectId == project.ProjectId);

            if (projectToDelete != null)
            {
                projectToDelete.ProjectDelete = "Y";
                context.SaveChanges();
                TempData["result"] = "Xóa thành công";
            }
            else
            {
                TempData["result"] = "Không tìm thấy dự án để cập nhật";
            }
            //context.Projects.Remove(project);
            //context.SaveChanges();
            //TempData["result"] = "Xóa thành công";

            return RedirectToAction("Index");
        }

    


    }
}
