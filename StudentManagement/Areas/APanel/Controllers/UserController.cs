using Microsoft.AspNetCore.Mvc;
using StudentManagement.Controllers;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Areas.APanel.Controllers
{
    [Area("APanel")]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(
    IUserService service,
    ApplicationDbContext context,
    ILogger<UserController> logger)
        {
            _service = service;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }
        [HttpGet]
        public JsonResult IsUserNameAvailable(string userName, long id)
        {
            var exists = _context.User.Any(x =>
                x.UserName == userName &&
                x.Id != id);

            return Json(!exists); 
        }

        [HttpGet]
        public IActionResult Create(long id)
        {
            try
            {
                LoadRoles();

                if (id > 0)
                {
                    var user = _service.GetById(id);

                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "User not found";
                        return RedirectToAction(nameof(Index));
                    }

                    return View(new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        RoleId = user.RoleId,
                        IsActive = user.IsActive
                    });
                }

                return View(new UserViewModel());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while loading user form.";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            try
            {
                LoadRoles();

                if (_service.UserNameExists(model.UserName, model.Id))
                {
                    ModelState.AddModelError("UserName", "User name already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                _service.AddOrUpdate(model);

                TempData["SuccessMessage"] =
                    model.Id > 0
                    ? "User updated successfully!"
                    : "User created successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                LoadRoles();
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        public IActionResult Details(long id)
        {
            var data = _service.GetUserDetails(id);

            if (data == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("Index");
            }

            return View(data);
        }
        private void LoadRoles()
        {
            ViewBag.Roles = _context.Role.ToList();
        }

        [HttpGet]
        public IActionResult ResetPassword(long id)
        {
            var user = _service.GetById(id);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.ChangePassword(model.Id, model.Password);

            TempData["SuccessMessage"] = "Password changed successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
