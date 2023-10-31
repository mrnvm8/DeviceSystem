using DeviceSystem.Models;

namespace DeviceSystem.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<int> Register(User user);
        Task<User?> Login(Guid employeeId);
        Task<bool> ExistAsync(Guid employeeId);
    }
}
