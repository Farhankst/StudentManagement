namespace StudentManagement.Models.Entities
{
    public class Teacher : BaseEntity
    {
        public string TeacherCode { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string? Mobile { get; set; } = string.Empty;

        public string? Designation { get; set; } = string.Empty;

        public long DepartmentId { get; set; }

        public DateTime JoiningDate { get; set; }

        public long UserId { get; set; }

    }
}
