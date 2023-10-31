using DeviceSystem.Models;

namespace DeviceSystem.Repositories.DepartmentRepository
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(Department department);
        Task<int> UpdateAsync(Department department);
        Task<int> DeleteAsync(Department department);
        Task<bool> ExistAsync(Department department);
    }
}
