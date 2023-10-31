using DeviceSystem.Requests.DeviceLoans;

namespace DeviceSystem.Services.DeviceLoanService
{
    public interface IDeviceLoanService
    {
        Task<ServiceResponse<List<DeviceLoanResponse>>> GetDeviceLoans();
        Task<ServiceResponse<List<DeviceLoanResponse>>> GetAllDeviceLoansById(Guid deviceId);
        Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(Guid deviceLoanId);
        Task<ServiceResponse<DeviceLoanResponse>> AssignDevice(CreateDeviceLoanRequest request);
        Task<ServiceResponse<DeviceLoanResponse>> UnassignedDevice(Guid deviceLoanId, UpdateDeviceLoanRequest request);
    }
}
