namespace DeviceSystem.Response_DTO_
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public  Roles Role { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
