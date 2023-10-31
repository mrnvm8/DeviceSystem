using DeviceSystem.Models;
namespace DeviceSystem.Requests.Device
{
    public class CreateDeviceRequest
    {
        public Guid DepartId { get; set; }
        public Guid TypeId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string? SerialNo { get; set; }
        public string? IMEINo { get; set; }
        public decimal PurchasedPrice { get; set; }
        public DateTime PurchasedDate { get; set; }
        public Condition Condition { get; set; }
    }
}
