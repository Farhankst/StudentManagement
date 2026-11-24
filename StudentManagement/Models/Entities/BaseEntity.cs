namespace StudentManagement.Models.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;

        public long? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        

        
    }
}
