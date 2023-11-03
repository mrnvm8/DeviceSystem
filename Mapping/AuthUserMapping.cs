using System.Security.Cryptography;

namespace DeviceSystem.Mapping
{
    public static class AuthUserMapping
    {

        public static UserResponse MapToResponse(this User user)
        {
            
            return new UserResponse
            {
                UserId = user.Id,
                EmployeeId = user.EmployeeId,
                DateCreated = user.DateCreated,
                Role = user.Role,
                Email = user.Employee!.WorkEmail,
                FullName = $"{user.Employee.Person?.FirstName}, {user.Employee.Person?.LastName}",
                Department = user.Employee.Department?.DepartmentName,
                Office = user.Employee.Department!.Offices?.Name
            };
        }

        public static User CreateMapToUser(this CreateUserRequest request, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            return new User
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                Role = request.Role,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                DateCreated = DateTime.Now
            };
        }

        public static User MapDbUser(this User user, string newPassword)
        {
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            return new User
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                Role = user.Role,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                DateCreated = user.DateCreated
            };
        }

        //creating a passwordhash when registering a user
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //crytogrphy algorithm
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                //creating password hash
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
