using Microsoft.AspNetCore.Http.HttpResults;
using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;
using System.Security.Principal;

namespace StudentManagement.Services.Implementations
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(ApplicationDbContext context, IHttpContextAccessor accessor, ILogger<DepartmentService> logger)
     : base(accessor)
        {
            _context = context;
            _logger = logger;
        }

        public List<Department> GetAll()
        {
            try
            {
                return _context.Department.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting department list: " + ex.Message);
            }
        }

        public Department GetById(long id)
        {
            try
            {
                return _context.Department.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting department: " + ex.Message);
            }
        }

        public void AddOrUpdate(Department model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var entity = _context.Department.FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        throw new Exception("Department not found");
                    }

                    entity.Code = model.Code;
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.IsActive = model.IsActive;
                    entity.UpdatedBy = GetUserId();
                    entity.UpdatedDate = DateTime.Now;
                }
                else
                {
                    var entity = new Department
                    {
                        Code = model.Code,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        CreatedBy = GetUserId(),
                        CreatedDate = DateTime.Now
                    };

                    _context.Department.Add(entity);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving department: " + ex.Message);
            }
        }
        public bool IsCodeDuplicate(string code, long id)
        {
            return _context.Department.Any(x =>
                x.Id != id && x.Code == code
            );
        }

        public bool IsNameDuplicate(string name, long id)
        {
            return _context.Department.Any(x =>x.Id != id && x.Name == name);
        }

    }
}
