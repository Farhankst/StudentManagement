namespace StudentManagement.Models.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public long RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
