using Microsoft.AspNetCore.Mvc;
using StudentManagement.Controllers;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Areas.APanel.Controllers
{
    [Area("APanel")]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            try
            {
                var data = _service.GetAll();
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong: " + ex.Message;
                return View(new List<Department>());
            }
        }
        [HttpGet]
        public JsonResult CheckDuplicate(string code, string name, long id = 0)
        {
            var codeExists = _service.IsCodeDuplicate(code, id);
            var nameExists = _service.IsNameDuplicate(name, id);

            return Json(new
            {
                codeExists,
                nameExists
            });
        }

        [HttpGet]
        public IActionResult Create(long id)
        {
            try
            {
                if (id > 0)
                {
                    var data = _service.GetById(id);

                    if (data == null)
                    {
                        TempData["ErrorMessage"] = "Department not found!";
                        return RedirectToAction("Index");
                    }

                    return View(data);
                }

                return View(new Department());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(Department model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var isCodeDuplicate = _service.IsCodeDuplicate(model.Code, model.Id);
                var isNameDuplicate = _service.IsNameDuplicate(model.Name, model.Id);

                if (isCodeDuplicate || isNameDuplicate)
                {
                    if (isCodeDuplicate)
                        ModelState.AddModelError("Code", "Code already exists!");

                    if (isNameDuplicate)
                        ModelState.AddModelError("Name", "Name already exists!");

                    return View(model);
                }

                _service.AddOrUpdate(model);

                TempData["SuccessMessage"] =
                    model.Id > 0 ? "Updated successfully!" : "Created successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public IActionResult Details(long id)
        {
            try
            {
                var department = _service.GetById(id);

                if (department == null)
                {
                    TempData["ErrorMessage"] = "Department not found!";
                    return RedirectToAction("Index");
                }

                return View(department);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
