using DeviceSystem.Models;

namespace DeviceSystem.Repositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<List<User>> GetUsers();
        Task<User?> GetUserById(Guid userId);
        Task<int> Register(User user);
        Task<int> ChangePassword(User user);
        Task<int> DeleteUser(User user);
        Task<User?> Login(Guid employeeId);
        Task<bool> ExistAsync(Guid employeeId);
    }
}
