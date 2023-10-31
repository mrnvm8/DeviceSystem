using DeviceSystem.Requests.Employee;

namespace DeviceSystem.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<EmployeeResponse>>> GetEmployees();
        Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(Guid employeeId);
        Task<ServiceResponse<bool>> CreateEmployee(CreateEmployeeRequest request);
        Task<ServiceResponse<bool>> UpdateEmployee(Guid employeeId, UpdateEmployeeRequest request);
        Task<ServiceResponse<bool>> RemoveEmployee(Guid employeeId);

        //This for getting all device assigned to a specify employee
        Task<ServiceResponse<IEnumerable<DeviceLoanResponse>>> GetDeviceLoanById(Guid employeeId);
    }
}