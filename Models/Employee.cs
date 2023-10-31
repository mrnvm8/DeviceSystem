namespace DeviceSystem.Models
{
    public class Employee
    {
        public Guid Id { get; init; }
        public Guid PersonId { get; init; }
        public Guid DepartmentId { get; init; }
        public string WorkEmail { get; init; } = string.Empty;
        public DateTime Enrollment { get; init; }
        public bool IsEmployeeActive { get; init; } = true;
        public virtual Person? Person { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public Employee()
        {
            Users = new HashSet<User>();
            DevicesLoans = new HashSet<DeviceLoans>();
        }
    }
}
