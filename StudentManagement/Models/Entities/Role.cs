using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.Entities
{
    public class Role : BaseEntity
    {
        [Required(ErrorMessage = "Role Name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
