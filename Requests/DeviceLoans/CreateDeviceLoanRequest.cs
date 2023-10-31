namespace DeviceSystem.Requests.DeviceLoans
{
    public class CreateDeviceLoanRequest
    {
        public Guid DeviceId { get; set; }
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsApproved { get; set; }

    }
}
