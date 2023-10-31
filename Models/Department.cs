namespace DeviceSystem.Models
{
    public class Department
    {
        public Guid Id { get; init; }
        public Guid OfficeId { get; init; }
        public string DepartmentName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public virtual Office? Offices { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public Department()
        {
            Employees = new HashSet<Employee>();
            Devices = new HashSet<Device>();
        }
    }
}
