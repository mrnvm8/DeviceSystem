using DeviceSystem.Mapping;
using DeviceSystem.Repositories.AuthRepository;
using DeviceSystem.Repositories.EmployeeRespository;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DeviceSystem.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmployeeRespository _employeeRespository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IAuthRepository authRepository,
            IEmployeeRespository employeeRespository,
            IHttpContextAccessor httpContextAccessor)
        {
            _authRepository = authRepository;
            _employeeRespository = employeeRespository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetEmployeeId() => Guid.Parse(_httpContextAccessor.HttpContext!.User
                   .FindFirstValue(ClaimTypes.Name)!);
        public Guid GetUserId() => Guid.Parse(_httpContextAccessor.HttpContext!.User
                   .FindFirstValue(ClaimTypes.NameIdentifier)!);
        public string GetUserRole() => _httpContextAccessor.HttpContext!.User
                   .FindFirstValue(ClaimTypes.Role)!;
        public async Task<ServiceResponse<UserResponse>> Login(string email, string password)
        {
            var response = new ServiceResponse<UserResponse>();
            //Get the user that have this email address in the
            //table Employee
            var employee = await _employeeRespository.GetEmployeeByEmailAsync(email);
            //if email does not exist return
            if (employee is null)
            {
                response.Success = false;
                response.Message = "User email does not exist";
                return response;
            }

            //checking if the user exist in the table User
            //using employeeId from the employee
            var user = await _authRepository.Login(employee.Id);

            if (user is null)
            {
                response.Success = false;
                response.Message = "User not Found";

            }
            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = user.MapToResponse();
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Register(CreateUserRequest request, string password)
        {
            var response = new ServiceResponse<string>();
            //mapping from request to user
            var user = request.CreateMapToUser(password);
            //check if the user exist in the user
            //table before registering them
            var userExist = await _authRepository.ExistAsync(user.EmployeeId);
            if (userExist)
            {
                response.Success = false;
                response.Message = "User already exist.";
                return response;
            }
            else
            {
                var affected = await _authRepository.Register(user);
                if (affected == 1)
                {
                    response.Message = "Registration successful.";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Registration Fail";
                    return response;
                }
            }
        }

        //Verifying PasswordHash when login
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);

            }
        }
    }
}
