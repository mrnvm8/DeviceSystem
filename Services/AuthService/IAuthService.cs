using DeviceSystem.Requests.User;

namespace DeviceSystem.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(CreateUserRequest request, string password);
        Task<ServiceResponse<UserResponse>> Login(string email, string password);
        Guid GetEmployeeId();
        Guid GetUserId();
        string GetUserRole();
    }
}
