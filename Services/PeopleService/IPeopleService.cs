using DeviceSystem.Requests.Person;

namespace DeviceSystem.Services.PersonService
{
    public interface IPeopleService
    {
        Task<ServiceResponse<List<PersonResponse>>> GetPeopleList();
        Task<ServiceResponse<PersonResponse>> GetPersonById(Guid personId);
        Task<ServiceResponse<bool>> CreatePerson(CreatePersonRequest request);
        Task<ServiceResponse<bool>> UpdatePerson(Guid personId, UpdatePersonRequest request);
        Task<ServiceResponse<bool>> RemovePerson(Guid personId);
    }
}
