using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StudentManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected long CurrentUserId
        {
            get
            {
                var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt64(value);
            }
        }

        protected string CurrentUserName
        {
            get
            {
                return User.Identity?.Name ?? "";
            }
        }

        protected string CurrentUserRole
        {
            get
            {
                return User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            }
        }

        protected long CurrentRoleId
        {
            get
            {
                var value = User.FindFirst("RoleId")?.Value;
                return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt64(value);
            }
        }
    }
}
