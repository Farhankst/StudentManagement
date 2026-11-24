using StudentManagement.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models.ViewModels
{
    public class UserViewModel : IValidatableObject
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public long RoleId { get; set; }
        public string? RoleName { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == 0)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult(
                        "Password is required",
                        new[] { nameof(Password) });
                }

                if (Password != ConfirmPassword)
                {
                    yield return new ValidationResult(
                        "Password and Confirm Password do not match",
                        new[] { nameof(ConfirmPassword) });
                }
            }
        }
    }
}
