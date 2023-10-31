using DeviceSystem.Models;

namespace DeviceSystem.Repositories.DeviceTypeRepository
{
    public interface IDeviceTypeRepository
    {
        Task<IEnumerable<DeviceType?>> GetAllAsync();
        Task<DeviceType?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(DeviceType deviceType);
        Task<int> UpdateAsync(DeviceType deviceType);
        Task<int> DeleteAsync(DeviceType deviceType);
        Task<bool> ExistAsync(DeviceType deviceType);
    }
}
