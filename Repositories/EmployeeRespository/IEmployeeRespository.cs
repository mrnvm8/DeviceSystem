using DeviceSystem.Models;

namespace DeviceSystem.Repositories.EmployeeRespository
{
    public interface IEmployeeRespository
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(Guid employeeId);
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<int> CreateEmployeeAsync(Employee employee);
        Task<int> UpdateEmployeeAsync(Employee employee);
        Task<int> DeleteEmployeeAsync(Employee employee);
        Task<bool> ExistEmployeeAsync(Employee employee);
    }
}
