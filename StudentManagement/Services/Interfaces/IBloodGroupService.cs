using StudentManagement.Models.Entities;

namespace StudentManagement.Services.Interfaces
{
    public interface IBloodGroupService
    {
        List<BloodGroup> GetAll();
        BloodGroup GetById(long id);
        void AddOrUpdate(BloodGroup model);
        bool IsNameDuplicate(string name, long id);
    }
}
