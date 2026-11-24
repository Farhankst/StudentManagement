using StudentManagement.Models.Entities;

namespace StudentManagement.Services.Interfaces
{
    public interface IGenderService
    {
        List<Gender> GetAll();
        Gender GetById(long id);
        void AddOrUpdate(Gender model);
        bool IsNameDuplicate(string name, long id);
    }
}
