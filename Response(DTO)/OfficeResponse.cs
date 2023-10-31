using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public class OfficeResponse
    {
        public Guid Id { get; set; }
        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Office Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(250, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Office Location"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Location { get; set; } = string.Empty;
    }
}
