using DeviceSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceSystem.Response_DTO_
{
    public record DeviceResponse
    {
        public Guid Id { get; set; }
        public Guid DepartId { get; set; }
        public Guid TypeId { get; set; }

        [Required, StringLength(20, MinimumLength = 3),
        DataType(DataType.Text),
        Display(Name = "Device Brand Name"),
        ]
        public string DeviceName { get; set; } = string.Empty;
        public string? SerialNo { get; set; }
        public string? IMEINo { get; set; }

        [Range(1, 20000),
        Display(Name = "Purchased Price"),
        Column(TypeName = "decimal(18, 2)")]
        public decimal PurchasedPrice { get; set; }

        [DataType(DataType.Date),
        Display(Name = "Purchased Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchasedDate { get; set; }
        public Condition Condition { get; set; }
        [Display(Name = "Department Name")]
        public string? DepartmentName { get; set; }
        [Display(Name = "Device Type Name")]
        public string? TypeName { get; set; }
        [Display(Name = "Identity Number")]
        public string? IdentityNumber { get; set; }
        public string? FullName { get; set; }
    }
}
