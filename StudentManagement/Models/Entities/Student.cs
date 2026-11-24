namespace StudentManagement.Models.Entities
{
    public class Student :BaseEntity
    {
        public string StudentCode { get; set; } = string.Empty;

        public string RegistrationNo { get; set; } = string.Empty;

        public string RollNo { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public string MotherName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public string? PresentAddress { get; set; }

        public string? PermanentAddress { get; set; }

        public string? PhotoUrl { get; set; }

        public long GenderId { get; set; }

        public long DepartmentId { get; set; }

        public long? BloodGroupId { get; set; }

        public long AcademicSessionId { get; set; }

        public long SemesterId { get; set; }

        public DateTime AdmissionDate { get; set; }

        public long UserId { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual BloodGroup? BloodGroup { get; set; }

        public virtual Department Department { get; set; }

        public virtual AcademicSession AcademicSession { get; set; }

        public virtual Semester Semester { get; set; }

        public virtual User User { get; set; }
    }
}
