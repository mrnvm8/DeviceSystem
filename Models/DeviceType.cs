namespace DeviceSystem.Models
{
    public class DeviceType
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public virtual ICollection<Device>? Devices { get; set; }
        public DeviceType()
        {
            Devices = new HashSet<Device>();
        }
    }
}
