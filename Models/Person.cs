namespace DeviceSystem.Models
{
    public class Person
    {
        public Guid Id { get; init; } 
        public string FirstName { get;init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public Gender Gender { get; init; } 
        public virtual ICollection<Employee> Employees { get; set; }
        public Person()
        {
            Employees = new HashSet<Employee>();
        }

    }
}
