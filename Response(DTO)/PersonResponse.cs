using DeviceSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public class PersonResponse
    {
        public Guid Id { get; init; }

        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "First Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Last Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
    }
}
