using DeviceSystem.Requests.User;

namespace DeviceSystem.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(CreateUserRequest request, string password);
        Task<ServiceResponse<string>> ChangeUserPassword(Guid userId, string newPassword);
        Task<ServiceResponse<UserResponse>> Login(string email, string password);
        Task<ServiceResponse<List<string>>> ValidatePassword(string password);
        Task<ServiceResponse<List<UserResponse>>> GetUsersFromDB();
        Task<ServiceResponse<UserResponse>> GetUserById(Guid userId);
        Task<ServiceResponse<bool>> RemoveUser(Guid userId);
        Guid GetEmployeeId();
        Guid GetUserId();
        string GetUserRole();
    }
}
