namespace DeviceSystem.Requests.Employee
{
    public class UpdateEmployeeRequest
    {
        public Guid PersonId { get; set; }
        public Guid DepartmentId { get; set; }
        public string WorkEmail { get; set; } = string.Empty;
        public DateTime Enrollment { get; set; }
        public bool IsEmployeeActive { get; set; } 
    }
}
