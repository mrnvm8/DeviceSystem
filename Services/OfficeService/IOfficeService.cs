using DeviceSystem.Requests.Office;

namespace DeviceSystem.Services.OfficeService
{
    public interface IOfficeService
    {
        Task<ServiceResponse<IEnumerable<OfficeResponse>>> GetOfficesList();
        Task<ServiceResponse<OfficeResponse>> GetOfficeById(Guid officeId);
        Task<ServiceResponse<bool>> CreateOffice(CreateOfficeRequest request);
        Task<ServiceResponse<bool>> UpdateOffice(Guid officeId, UpdateOfficeRequest request);
        Task<ServiceResponse<bool>> RemoveOffice(Guid officeId);
    }
}

