using DeviceSystem.Models;

namespace DeviceSystem.Repositories.PeopleRepository
{
    public interface IPeopleRepository
    {
        Task<List<Person>> GetPeopleAsync();
        Task<Person?> GetPersonByIdAsync(Guid personId);
        Task<int> CreatePersonAsync(Person person);
        Task<int> UpdatePersonAsync(Person person);
        Task<int> DeletePersonAsync(Person person);
        Task<bool> ExistPersonAsync(Person person);
    }
}
