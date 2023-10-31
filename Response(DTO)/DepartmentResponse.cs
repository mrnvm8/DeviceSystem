using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public class DepartmentResponse
    {
        public Guid Id { get; set; }
        public Guid OfficeId { get; set; }

        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Department Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required, StringLength(250, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Department Description"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Office Name")]
        public string? OfficeName { get; set; }

    }
}
