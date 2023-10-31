using DeviceSystem.Requests.Department;

namespace DeviceSystem.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<ServiceResponse<List<DepartmentResponse>>> GetDepartmentsList();
        Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(Guid departmentId);
        Task<ServiceResponse<bool>> CreateDepartment(CreateDepartmentRequest request);
        Task<ServiceResponse<bool>> UpdateDepartment(Guid departmentId, UpdateDepartmentRequest request);
        Task<ServiceResponse<bool>> RemoveDepartment(Guid departmentId);
    }
}
