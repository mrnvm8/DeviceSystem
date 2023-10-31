using DeviceSystem.Requests.DeviceType;

namespace DeviceSystem.Services.DeviceTypeService
{
    public interface IDeviceTypeService
    {
        Task<ServiceResponse<IEnumerable<DeviceTypeResponse>>> GetDeviceTypesList();
        Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(Guid typeId);
        Task<ServiceResponse<bool>> AddDeviceType(CreateTypeRequest request);
        Task<ServiceResponse<bool>> UpdateDeviceType(Guid typeId, UpdateTypeRequest request);
        Task<ServiceResponse<bool>> RemoveDeviceType(Guid typeId);
    }
}
