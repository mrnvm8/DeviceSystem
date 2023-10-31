namespace DeviceSystem.Models
{
    public class DeviceLoans
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid DeviceId { get; init; }
        public Guid EmployeeId { get; init; }
        public DateTime AssignedDate { get; set; }
        public Guid? ReturnToUserId { get; init; }
        public DateTime ReturnDate { get; set; }
        public bool IsApproved { get; set; }
        public virtual User? User { get; set; }
        public virtual Device? Device { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
