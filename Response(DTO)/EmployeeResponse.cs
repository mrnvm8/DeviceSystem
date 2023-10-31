using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public  Guid DepartmentId { get; set; }

        [Required,
        EmailAddress,
        Display(Name = "Work Email")]
        public string WorkEmail { get; set; } = string.Empty;

        [DataType(DataType.Date),
        Display(Name = "Enroment Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Enrollment { get; set; }
        [Display(Name = "Employee Name")]
        public string? FullName { get; set; }
        [Display(Name = "Department")]
        public string? DepartmentName { get; set; }
        [Display(Name = "Office Name")]
        public string? OfficeName { get; set; }

    }
}
