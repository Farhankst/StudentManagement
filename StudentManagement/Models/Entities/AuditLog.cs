namespace StudentManagement.Models.Entities
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string TableName { get; set; }
        public string ActionType { get; set; }

        public string? KeyValue { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public long? UserId { get; set; }        
        public string? IpAddress { get; set; }    
        public string? ControllerName { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}
