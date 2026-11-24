using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.Entities
{
    public class Department : BaseEntity
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department Name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
