using StudentManagement.Data;
using StudentManagement.Models.Entities;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Implementations
{
    public class GenderService : BaseService, IGenderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GenderService> _logger;

        public GenderService(ApplicationDbContext context, IHttpContextAccessor accessor, ILogger<GenderService> logger)
     : base(accessor)
        {
            _context = context;
            _logger = logger;
        }

        public List<Gender> GetAll()
        {
            try
            {
                return _context.Gender.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting gender list: " + ex.Message);
            }
        }

        public Gender GetById(long id)
        {
            try
            {
                return _context.Gender.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting gender: " + ex.Message);
            }
        }

        public void AddOrUpdate(Gender model)
        {
            try
            {
                if (model.Id > 0)
                {
                    var entity = _context.Gender.FirstOrDefault(x => x.Id == model.Id);

                    if (entity == null)
                    {
                        throw new Exception("Gender not found");
                    }

                    entity.Name = model.Name;
                    entity.IsActive = model.IsActive;
                    entity.UpdatedBy = GetUserId();
                    entity.UpdatedDate = DateTime.Now;
                }
                else
                {
                    var entity = new Gender
                    {
                        Name = model.Name,
                        IsActive = model.IsActive,
                        CreatedBy = GetUserId(),
                        CreatedDate = DateTime.Now
                    };

                    _context.Gender.Add(entity);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving Gender: " + ex.Message);
            }
        }

        public bool IsNameDuplicate(string name, long id)
        {
            return _context.Gender.Any(x => x.Id != id && x.Name == name);
        }
    }
}
