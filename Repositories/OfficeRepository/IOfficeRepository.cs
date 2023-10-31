using DeviceSystem.Models;

namespace DeviceSystem.Repositories.OfficeRepository
{
    public interface IOfficeRepository
    {
        Task<IEnumerable<Office?>> GetAllAsync();
        Task<Office?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(Office office);
        Task<int> UpdateAsync(Office office);
        Task<int> DeleteAsync(Office office);
        Task<bool> ExistAsync(Office office);
    }
}
