using DeviceSystem.Models;

namespace DeviceSystem.Repositories.DeviceLoanRepository
{
    public interface IDeviceLoanRepository
    {
        Task<List<DeviceLoans>> GetAllAsync();
        Task<DeviceLoans?> GetByIdAsync(Guid deviceId);
        Task<int> CreateAsync(DeviceLoans deviceLoans);
        Task<int> UpdateAsync(DeviceLoans deviceLoans);
        Task<List<DeviceLoans>> GetDeviceLoansByDeviceId(Guid deviceId);
        Task<List<DeviceLoans>> GetDeviceLoansByEmployeeId(Guid employeeId);
    }
}
