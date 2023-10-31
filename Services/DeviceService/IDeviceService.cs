using DeviceSystem.Models;
using DeviceSystem.Requests.Device;

namespace DeviceSystem.Services.DeviceService
{
    public interface IDeviceService
    {
        Task<ServiceResponse<List<DeviceResponse>>> GetDevices();
        Task<ServiceResponse<List<DeviceResponse>>> GetDevicesList(Guid departmentId, Guid DeviceTypeId);
        Task<ServiceResponse<DeviceResponse>> GetDeviceById(Guid deviceId);
        Task<ServiceResponse<DeviceResponse>> AddDevice(CreateDeviceRequest request);
        Task<ServiceResponse<DeviceResponse>> UpdateDevice(Guid deviceId, UpdateDeviceRequest request);
        Task<ServiceResponse<bool>> RemoveDevice(Guid deviceId);
        Task<ServiceResponse<List<DeviceSummary>>> Summary();
    }
}
