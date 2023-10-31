using System.ComponentModel.DataAnnotations;

namespace DeviceSystem.Response_DTO_
{
    public class DeviceLoanResponse
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; } 
        public Guid EmployeeId { get; set; }

        [DataType(DataType.Date),
        Display(Name = "Assigned Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date),
        Display(Name = "Return Date"),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReturnDate { get; set; }
        public string? DepartmentName { get; set; }
        
        public bool IsApproved { get; set; } = false;
        public string? EmployeeName { get; set; }
        public Guid? DepartmentId {get; set;}
        public Guid? DeviceTypeId { get; set;}
        
        public Device LoanDevice {get; set;} = new Device();

    }
}
