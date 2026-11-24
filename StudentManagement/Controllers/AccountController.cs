using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Helpers;
using System.Security.Claims;

namespace StudentManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = _context.User.FirstOrDefault(x => x.UserName == username);

                if (user == null)
                {
                    ViewBag.Error = "Invalid username or password";
                    return View();
                }

                bool isValidPassword =PasswordHelper.VerifyPassword(password, user.PasswordHash);

                if (!isValidPassword)
                {
                    ViewBag.Error = "Invalid username or password";
                    return View();
                }

                var role = _context.Role.FirstOrDefault(x => x.Id == user.RoleId);

                if (role == null)
                {
                    ViewBag.Error = "Role not found";
                    return View();
                }

                var claims = new List<Claim>
        {
            // Standard Claims
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, role.Name),

            // Custom Claims
            new Claim("UserId", user.Id.ToString()),
            new Claim("RoleId", role.Id.ToString()),
            new Claim("UserName", user.UserName)
        };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                if (role.Name == "SuperAdmin" || role.Name == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "APanel" });
                }

                if (role.Name == "Student" || role.Name == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "CPanel" });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Denied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            TempData["ForgotMsg"] = true;
            return RedirectToAction("Login");
        }
    }
}
