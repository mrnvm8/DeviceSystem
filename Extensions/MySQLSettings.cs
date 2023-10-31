namespace DeviceSystem.Extensions
{
    public class MySQLSettings
    {
        public string Server { get; init; } = string.Empty;
        public string User { get; init; } = string.Empty;
        public string Database { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string ConnectionString
        {
            get
            {
                return $"server={Server};user={User};database={Database};password={Password}";
            }
        }
    }
}
