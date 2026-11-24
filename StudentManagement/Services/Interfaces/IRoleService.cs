using StudentManagement.Models.Entities;

namespace StudentManagement.Services.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetAll();
        Role GetById(long id);
        void AddOrUpdate(Role model);
        bool IsNameDuplicate(string name, long id);
    }
}
