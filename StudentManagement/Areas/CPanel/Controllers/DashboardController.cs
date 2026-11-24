using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Areas.CPanel.Controllers
{
    public class DashboardController : Controller
    {
        [Area("CPanel")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
