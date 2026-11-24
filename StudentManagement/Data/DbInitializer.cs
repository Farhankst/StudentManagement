using StudentManagement.Helpers;
using StudentManagement.Models.Entities;

namespace StudentManagement.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();

            // =========================
            // ROLE SEED
            // =========================
            if (!context.Role.Any())
            {
                context.Role.AddRange(
                    new Role { Name = "SuperAdmin", IsActive = true, CreatedDate = DateTime.Now },
                    new Role { Name = "Admin", IsActive = true, CreatedDate = DateTime.Now },
                    new Role { Name = "Teacher", IsActive = true, CreatedDate = DateTime.Now },
                    new Role { Name = "Student", IsActive = true, CreatedDate = DateTime.Now },
                    new Role { Name = "Accountant", IsActive = true, CreatedDate = DateTime.Now }
                );

                context.SaveChanges();
            }

            // =========================
            // GENDER SEED
            // =========================
            if (!context.Gender.Any())
            {
                context.Gender.AddRange(
                    new Gender { Name = "Male", IsActive = true, CreatedDate = DateTime.Now },
                    new Gender { Name = "Female", IsActive = true, CreatedDate = DateTime.Now },
                    new Gender { Name = "Other", IsActive = true, CreatedDate = DateTime.Now }
                );

                context.SaveChanges();
            }

            // =========================
            // BLOOD GROUP SEED
            // =========================
            if (!context.BloodGroup.Any())
            {
                context.BloodGroup.AddRange(
                    new BloodGroup { Name = "A+", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "A-", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "B+", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "B-", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "AB+", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "AB-", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "O+", IsActive = true, CreatedDate = DateTime.Now },
                    new BloodGroup { Name = "O-", IsActive = true, CreatedDate = DateTime.Now }
                );

                context.SaveChanges();
            }

            // =========================
            // SUPER ADMIN USER SEED
            // =========================
            if (!context.User.Any())
            {
                var superAdminRoleId = context.Role.First(x => x.Name == "SuperAdmin").Id;

                context.User.Add(new User
                {
                    UserName = "admin",
                    PasswordHash = PasswordHelper.HashPassword("123456"),
                    FullName = "System Administrator",
                    Email = "admin@system.com",
                    RoleId = superAdminRoleId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }
}
