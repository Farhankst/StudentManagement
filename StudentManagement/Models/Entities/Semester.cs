namespace StudentManagement.Models.Entities
{
    public class Semester : BaseEntity
    {
        public long DepartmentId { get; set; }

        public int SemesterNo { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Credit { get; set; }

        public virtual Department Department { get; set; }
    }
}
