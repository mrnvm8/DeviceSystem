using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public record DeviceTypeResponse
    {
        public Guid Id { get; set; }
        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Device Type Name"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(250, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Description Of Type"),
        RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$",
         ErrorMessage = "Characters are not allowed.")]
        public string Description { get; set; } = string.Empty;
    }
}
