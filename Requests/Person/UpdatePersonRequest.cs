namespace DeviceSystem.Requests.Person
{
    public class UpdatePersonRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; }
    }
}
