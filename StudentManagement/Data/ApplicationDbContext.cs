using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models.Entities;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text.Json;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor
    ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Department> Department { get; set; }
        public DbSet<AcademicSession> AcademicSession { get; set; }
        public DbSet<Semester> Semester { get; set; }

        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<BloodGroup> BloodGroup { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany()
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Gender)
                .WithMany()
                .HasForeignKey(s => s.GenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.BloodGroup)
                .WithMany()
                .HasForeignKey(s => s.BloodGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.AcademicSession)
                .WithMany()
                .HasForeignKey(s => s.AcademicSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Semester)
                .WithMany()
                .HasForeignKey(s => s.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            var auditEntries = new List<AuditLog>();

            var httpContext = _httpContextAccessor.HttpContext;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached)
                    continue;
                var keyProperty = entry.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();

                var audit = new AuditLog
                {
                    TableName = entry.Entity.GetType().Name,
                    CreatedDate = DateTime.Now,
                    KeyValue = entry.State == EntityState.Added? null: keyProperty != null? entry.Property(keyProperty.Name).CurrentValue?.ToString(): null,
                    UserId = long.TryParse(
                        httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        out var uid) ? uid : null,
                    IpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString(),
                    ControllerName = httpContext?.GetRouteData()?.Values["controller"]?.ToString()
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        audit.ActionType = "INSERT";
                        audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        audit.OldValues = null;
                        break;

                    case EntityState.Modified:
                        audit.ActionType = "UPDATE";
                        audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        audit.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        break;

                    case EntityState.Deleted:
                        audit.ActionType = "DELETE";
                        audit.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        audit.NewValues = null;
                        break;

                    default:
                        continue;
                }

                auditEntries.Add(audit);
            }

            var result = base.SaveChanges();

            if (auditEntries.Any())
            {
                AuditLog.AddRange(auditEntries);
                base.SaveChanges();
            }

            return result;
        }
    }

}
