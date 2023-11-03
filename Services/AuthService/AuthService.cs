using DeviceSystem.Mapping;
using DeviceSystem.Repositories.AuthRepository;
using DeviceSystem.Repositories.EmployeeRespository;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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

            //lets valid the password 
            //get list of error
            var validate = ValidatePassword(password).Result;
            if(validate.Data!.Count() != 0)
            {
                response.Success =false;
                var _messages = string.Empty;
                //convert list to string
                foreach(var item in validate.Data!){
                   _messages = item + "," +_messages;
                }
    
                response.Message = _messages;
                return response;
            }

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

        public Task<ServiceResponse<List<string>>> ValidatePassword(string password)
        {
            var response = new ServiceResponse<List<string>>();
            var _messages = new List<string>();

            //break down the reg into pattern to list
            var patterns = new List<string>
            {
                "(?=^.{8,15}$)", "(?=.*?[A-Z])", "(?=.*?[a-z])",
                "(?=.*?[0-9])", "(?=.*?[#?!@$%^&*-])"
            };

            //Error message of the pattern to list
            var ErrorMessagse = new List<string>
            {
                "Password must at least have 8 -15 characters in length.",
                "Password must at least have one uppercase letter (A-Z).",
                "Password must at least have one lowercase letter (a-z).",
                "Password must at least have one digit (0-9).",
                "Password must at least have one special character (@,#,%,&,!,$,etc…).",
            };

            //looping through the pattern list match with the password
            //if match is true continue, if match false display the error
            for (var x = 0; x < patterns.Count; x++)
            {
                var validate = new Regex(patterns[x]);

                if (!validate.IsMatch(password))
                {
                    _messages.Add(ErrorMessagse[x]);
                }
                else
                {
                    continue;
                }
            }

            //checking for consecutive repeating characters
            if(ConsecutiveRepeatingCh(password)){
                var message = "Password can not have consecutive repeating characters or digit";
                _messages.Add(message);
            }else if(BlacklistedWords(password))
            {
                var message = "Password contains blacklisted words or digits e.g 123 or axium";
                _messages.Add(message);
            }

            response.Data = _messages;
            return Task.FromResult(response);
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

        //checking for repeating of character or digit 
        //eg. pas666 or paaa@1
        public bool ConsecutiveRepeatingCh(string password)
        {
            for (var x = 0; x < password.Length - 2; x++)
            {
                if (password[x].Equals(password[x + 1]) && password[x].Equals(password[x + 2]))
                {
                    return true;
                }
            }
            return false;
        }

        //Forbidden words as password
        public bool BlacklistedWords(string password)
        {
            var blacklisted = new List<string> { "123", "password", "axium", "admin", "administrator" };

            var validator = blacklisted
                            .Select(word => new Regex($@"\w*{Regex.Escape(word)}\w*", RegexOptions.IgnoreCase))
                            .ToList();
            if (validator.Any(valid => valid.IsMatch(password)))
            {
              return true;
            }
            return false;
        }

        public async Task<ServiceResponse<string>> ChangeUserPassword(Guid userId, string newPassword)
        {
            var response = new ServiceResponse<string>();
            //get user by Id
            var _user = await _authRepository.GetUserById(userId);

            if(_user is null){
                response.Success = false;
                response.Message ="User not Found";
                return response;
            }else{

                //TODO: lets valid the password 
                //get list of error
                var validate = ValidatePassword(newPassword).Result;
                if (validate.Data!.Count() != 0)
                {
                    response.Success = false;
                    var _messages = string.Empty;
                    //convert list to string
                    foreach (var item in validate.Data!)
                    {
                        _messages = item + "," + _messages;
                    }

                    response.Message = _messages;
                    return response;
                }

                //mapping user
                var newUser = _user.MapDbUser(newPassword);
                var affected  = await _authRepository.ChangePassword(newUser);
                if (affected == 1)
                {
                    response.Message = "Password was successful changed.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Fail to change password";
                }
            }
            return response;
        }

        public async Task<ServiceResponse<List<UserResponse>>> GetUsersFromDB()
        {
            var response = new ServiceResponse<List<UserResponse>>();
            var _usersList = new List<UserResponse>();

            //Get the list of users
            var _users = await _authRepository.GetUsers();

            if (_users.Count() == 0)
            {
                //if there are no users in the database
                response.Success = false;
                response.Message = "There are no users in the database yet.";
                return response;
            }
            else
            {
                _users.ForEach(user =>
                {
                    _usersList.Add(user.MapToResponse());
                });
                response.Data = _usersList;

                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveUser(Guid userId)
        {
            var response = new ServiceResponse<bool>();
            //Get the user from the database by id
            var user = await _authRepository.GetUserById(userId);
            if (user is null)
            {
                //if the user is not in the database then return false and error message
                response.Success = false;
                response.Message = "user not found.";
                return response;
            }
            else
            {
                //if them user exist delete the person
                var affected = await _authRepository.DeleteUser(user);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the user";
                    return response;
                }
            }
        }
        public async Task<ServiceResponse<UserResponse>> GetUserById(Guid userId)
        {
            var response = new ServiceResponse<UserResponse>();
            // Get the user from the database by Id
            var user = await _authRepository.GetUserById(userId);

            if (user is null)
            {
                //if the user is not in the database then return false and error message
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            else
            {
                response.Data = user.MapToResponse();
                return response;
            }
        }
    }
}
