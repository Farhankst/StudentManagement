using Microsoft.AspNetCore.Mvc;
using StudentManagement.Controllers;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Areas.APanel.Controllers
{
    [Area("APanel")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }


        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }
        [HttpGet]
        public JsonResult CheckDuplicate(string name, long id = 0)
        {
            var isDuplicate = _service.IsNameDuplicate(name, id);
            return Json(new { isDuplicate });
        }

        [HttpGet]
        public IActionResult Create(long id)
        {
            if (id > 0)
            {
                var data = _service.GetById(id);

                if (data == null)
                {
                    TempData["ErrorMessage"] = "Role not found!";
                    return RedirectToAction("Index");
                }

                return View(data);
            }

            return View(new Role());
        }

        [HttpPost]
        public IActionResult Create(Role model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (_service.IsNameDuplicate(model.Name, model.Id))
                {
                    ModelState.AddModelError("Name", "Role name already exists!");
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
            var data = _service.GetById(id);

            if (data == null)
            {
                TempData["ErrorMessage"] = "Role not found!";
                return RedirectToAction("Index");
            }

            return View(data);
        }
    }
}
