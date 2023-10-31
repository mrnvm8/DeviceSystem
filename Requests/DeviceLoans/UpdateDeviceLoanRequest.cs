namespace DeviceSystem.Requests.DeviceLoans
{
    public class UpdateDeviceLoanRequest
    {
        public Guid DeviceId { get; set; }
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime ReturnDate { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = true;
    }
}
