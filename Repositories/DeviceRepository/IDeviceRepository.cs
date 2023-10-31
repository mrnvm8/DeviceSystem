using DeviceSystem.Models;

namespace DeviceSystem.Repositories.DeviceRepository
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetDevicesList();
        Task<List<Device>> GetAllAsync(Guid departmentId, Guid DeviceTypeId);
        Task<Device?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(Device device);
        Task<int> UpdateAsync(Device device);
        Task<int> DeleteAsync(Device device);
        Task<bool> ExistAsync(Device device);
        Task<List<DeviceSummary>> DeviceSummary(Guid departmentId);
        // Task<List<DeviceSummary>> DeviceSummaryByDepartmentId();
    }
}
