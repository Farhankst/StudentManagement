using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class BloodGroupService : BaseService, IBloodGroupService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BloodGroupService> _logger;

        public BloodGroupService(ApplicationDbContext context, IHttpContextAccessor accessor, ILogger<BloodGroupService> logger)
     : base(accessor)
        {
            _context = context;
            _logger = logger;
        }

        public List<BloodGroup> GetAll()
        {
            try
            {
                return _context.BloodGroup.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting gender list: " + ex.Message);
            }
        }

        public BloodGroup GetById(long id)
        {
            try
            {
                return _context.BloodGroup.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting BloodGroup: " + ex.Message);
            }
        }

        public void AddOrUpdate(BloodGroup model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var entity = _context.BloodGroup.FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        throw new Exception("BloodGroup not found");
                    }

                    entity.Name = model.Name;
                    entity.IsActive = model.IsActive;
                    entity.UpdatedBy = GetUserId();
                    entity.UpdatedDate = DateTime.Now;
                }
                else
                {
                    var entity = new BloodGroup
                    {
                        Name = model.Name,
                        IsActive = model.IsActive,
                        CreatedBy = GetUserId(),
                        CreatedDate = DateTime.Now
                    };

                    _context.BloodGroup.Add(entity);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving BloodGroup: " + ex.Message);
            }
        }

        public bool IsNameDuplicate(string name, long id)
        {
            return _context.BloodGroup.Any(x => x.Id != id && x.Name == name);
        }
    }
}
