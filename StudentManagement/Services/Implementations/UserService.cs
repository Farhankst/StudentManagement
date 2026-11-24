using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Helpers;
using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, IHttpContextAccessor accessor, ILogger<UserService> logger)
     : base(accessor)
        {
            _context = context;
            _logger = logger;
        }

        public List<User> GetAll()
        {
            return _context.User.Include(x => x.Role).ToList();
        }

        public User? GetById(long id)
        {
            return _context.User.FirstOrDefault(x => x.Id == id);
        }

        public void AddOrUpdate(UserViewModel model)
        {
            try
            {
               
                if (model.Id > 0)
                {
                    var userDb = _context.User.FirstOrDefault(x => x.Id == model.Id);

                    if (userDb == null)
                    {
                        _logger.LogWarning("User not found with ID: {Id}", model.Id);
                        throw new Exception("User not found");
                    }

                    userDb.UserName = model.UserName;
                    userDb.RoleId = model.RoleId;
                    userDb.IsActive = model.IsActive;

                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        userDb.PasswordHash = PasswordHelper.HashPassword(model.Password);
                    }

                    userDb.UpdatedBy = GetUserId();
                    userDb.UpdatedDate = DateTime.Now;

                    _logger.LogInformation("User updated successfully: {UserName}", model.UserName);
                }
                else
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        PasswordHash = PasswordHelper.HashPassword(model.Password),
                        RoleId = model.RoleId,
                        IsActive = model.IsActive,
                        CreatedBy = GetUserId(),
                        CreatedDate = DateTime.Now
                    };

                    _context.User.Add(user);

                    _logger.LogInformation("New user created: {UserName}", model.UserName);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddOrUpdate UserService");
                throw;
            }
        }

        public bool UserNameExists(string userName, long id)
        {
            return _context.User.Any(x => x.UserName == userName && x.Id != id);
        }
        public UserViewModel GetUserDetails(long id)
        {
            return (from u in _context.User
                    join r in _context.Role on u.RoleId equals r.Id
                    where u.Id == id
                    select new UserViewModel
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        RoleName = r.Name,
                        IsActive = u.IsActive
                    }).FirstOrDefault();
        }
        public void ChangePassword(long id, string password)
        {
            var user = _context.User.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.PasswordHash = PasswordHelper.HashPassword(password);

            _context.SaveChanges();
        }
    }
}
