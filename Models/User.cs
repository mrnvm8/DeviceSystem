namespace DeviceSystem.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public Guid EmployeeId { get; init; }
        public byte[] PasswordHash { get; init; }
        public byte[] PasswordSalt { get; init; }
        public Roles Role { get; init; } 
        public DateTime DateCreated { get; init; } 
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public User() => DevicesLoans = new HashSet<DeviceLoans>();
    }
}
