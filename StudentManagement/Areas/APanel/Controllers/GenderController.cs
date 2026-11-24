using Microsoft.AspNetCore.Mvc;
using StudentManagement.Controllers;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Areas.APanel.Controllers
{
    [Area("APanel")]
    public class GenderController : BaseController
    {
        private readonly IGenderService _service;

        public GenderController(IGenderService service)
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
                return View(new List<Gender>());
            }
        }
        [HttpGet]
        public JsonResult CheckDuplicate(string name, long id = 0)
        {
            var nameExists = _service.IsNameDuplicate(name, id);

            return Json(new
            {
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
                        TempData["ErrorMessage"] = "Gender not found!";
                        return RedirectToAction("Index");
                    }

                    return View(data);
                }

                return View(new Gender());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(Gender model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var isNameDuplicate = _service.IsNameDuplicate(model.Name, model.Id);

                if (isNameDuplicate)
                {
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
                var gender = _service.GetById(id);

                if (gender == null)
                {
                    TempData["ErrorMessage"] = "Gender not found!";
                    return RedirectToAction("Index");
                }

                return View(gender);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
