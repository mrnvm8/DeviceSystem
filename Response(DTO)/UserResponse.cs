namespace DeviceSystem.Response_DTO_
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public  Roles Role { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Department { get; set; }
        public string? FullName {get; set;}
        public string? Office { get; set; }
    }
}
