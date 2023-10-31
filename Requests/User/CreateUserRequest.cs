using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Requests.User
{
    public class CreateUserRequest
    {
        public Guid EmployeeId { get; set; }

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The passwords do not match."),
        Display(Name = "Confirm Password")]
        
        public string ConfirmPassword { get; set; } = string.Empty;
        public Roles Role { get; set; }

    }
}
