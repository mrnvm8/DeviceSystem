namespace DeviceSystem.Models
{
    public class Device
    {
        public Guid Id { get; init; }
        public Guid DepartmentId { get; init; }
        public Guid DeviceTypeId { get; init; }
        public string DeviceName { get; init; } =string.Empty;
        public string DeviceSerialNo { get; init; } = string.Empty;
        public string DeviceIMEINo { get; init; } = string.Empty;
        public Condition Condition { get; init; }
        public decimal PurchasedPrice { get; init; }
        public DateTime PurchasedDate { get; init; }
        public int Year { get; init; }
        public virtual DeviceType? DeviceType { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<DeviceLoans> DevicesLoans { get; set; }
        public Device()
        {
            DevicesLoans = new HashSet<DeviceLoans>();
        }
    }
}
