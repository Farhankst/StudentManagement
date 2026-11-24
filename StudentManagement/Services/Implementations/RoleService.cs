using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleService> _logger;

        public RoleService(ApplicationDbContext context, IHttpContextAccessor accessor, ILogger<RoleService> logger)
     : base(accessor)
        {
            _context = context;
            _logger = logger;
        }
        public List<Role> GetAll()
        {
            try
            {
                return _context.Role.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting role list: " + ex.Message);
            }
        }

        public Role GetById(long id)
        {
            try
            {
                return _context.Role.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting role: " + ex.Message);
            }
        }
        public void AddOrUpdate(Role model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var entity = _context.Role.FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        throw new Exception("Role not found");
                    }

                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.IsActive = model.IsActive;
                    entity.UpdatedBy = GetUserId();
                    entity.UpdatedDate = DateTime.Now;
                }
                else
                {
                    var entity = new Role
                    {
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        CreatedBy = GetUserId(),
                        CreatedDate = DateTime.Now
                    };

                    _context.Role.Add(entity);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving role: " + ex.Message);
            }
        }
        public bool IsNameDuplicate(string name, long id)
        {
            return _context.Role.Any(x => x.Id != id && x.Name == name);
        }
    }
}
