namespace DeviceSystem.Requests.Employee
{
    public class CreateEmployeeRequest
    {
        public Guid PersonId { get; init; }
        public Guid DepartmentId { get; init; }
        public string WorkEmail { get; init; } = string.Empty;
        public DateTime Enrollment { get; init; }
        public bool IsEmployeeActive { get; init; }
        
    }
}
