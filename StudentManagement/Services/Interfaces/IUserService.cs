using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;

namespace StudentManagement.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User? GetById(long id);
        void AddOrUpdate(UserViewModel user);
        bool UserNameExists(string userName, long id);
        UserViewModel GetUserDetails(long id);
        void ChangePassword(long id, string password);
    }
}
