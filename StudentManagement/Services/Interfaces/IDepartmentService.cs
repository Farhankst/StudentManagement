using StudentManagement.Models.Entities;

namespace StudentManagement.Services.Interfaces
{
    public interface IDepartmentService
    {
        List<Department> GetAll();
        Department GetById(long id);
        void AddOrUpdate(Department model);
        bool IsCodeDuplicate(string code, long id);
        bool IsNameDuplicate(string name, long id);
    }
}
