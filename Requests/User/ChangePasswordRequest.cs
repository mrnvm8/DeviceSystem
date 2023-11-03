using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Requests.User
{
    public class ChangePasswordRequest
    {
        [Required, StringLength(15, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
