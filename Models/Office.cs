namespace DeviceSystem.Models
{
    public class Office
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public virtual ICollection<Department> Departments { get; set; }
        public Office()
        {
            Departments = new HashSet<Department>();
        }
    }
}
