using DeviceSystem.Mapping;
using DeviceSystem.Repositories.PeopleRepository;
using DeviceSystem.Requests.Person;

namespace DeviceSystem.Services.PersonService
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository;

        public PeopleService(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        public async Task<ServiceResponse<bool>> CreatePerson(CreatePersonRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to person Entity
            var person = request.CreateMapToPerson();
            //check if the person is already in the Database
            var exist = await _peopleRepository.ExistPersonAsync(person);
            if (exist)
            {
                //if the person is already in the database then return false and error message
                response.Success = false;
                response.Message = $"{person.FirstName}, {person.LastName} already exist in the People list.";
                return response;
            }
            else
            {
                //if the person is not in the database then create a new one
                var affected = await _peopleRepository.CreatePersonAsync(person);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create the Person";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<List<PersonResponse>>> GetPeopleList()
        {
            var response = new ServiceResponse<List<PersonResponse>>();
            var _peopleList = new List<PersonResponse>();
            
            //Get the list of people
            var _people = await _peopleRepository.GetPeopleAsync();

            if (_people.Count() == 0)
            {
                //if there are no people in the database
                response.Success = false;
                response.Message = "There are no people in the database yet.";
                return response;
            }
            else
            {
                //mapping each person to person response(dto)
                _people.ForEach(person => {
                    _peopleList.Add(person.MapToPersonResponse());
                });
                response.Data = _peopleList;

                return response;
            }
        }

        public async Task<ServiceResponse<PersonResponse>> GetPersonById(Guid personId)
        {
            var response = new ServiceResponse<PersonResponse>();
            // Get the person from the database by Id
            var person = await _peopleRepository.GetPersonByIdAsync(personId);

            if (person is null)
            {
                //if the person is not in the database then return false and error message
                response.Success = false;
                response.Message = "Person not found.";
                return response;
            }
            else
            {
                //if the person is in the database then map it to DTO 
                //and  return person Dto information
                response.Data = person.MapToPersonResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemovePerson(Guid personId)
        {
            var response = new ServiceResponse<bool>();
            //Get the person from the database by id
            var person = await _peopleRepository.GetPersonByIdAsync(personId);
            if (person is null)
            {
                //if the person is not in the database then return false and error message
                response.Success = false;
                response.Message = "Person not found.";
                return response;
            }
            else
            {
                //if the person exist delete the person
                var affected = await _peopleRepository.DeletePersonAsync(person);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the person";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> UpdatePerson(Guid personId, UpdatePersonRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to person Entity
            var person = request.UpdateMapToPerson(personId);

            //get the person from the database by its id
            var dbPerson = await _peopleRepository.GetPersonByIdAsync(personId);
            if (dbPerson is null)
            {
                //if the person is not in the database then return false and error message
                response.Success = false;
                response.Message = "Person not found.";
                return response;
            }
            else
            {
                dbPerson = new Person 
                {
                    Id= dbPerson.Id,
                    FirstName = person.FirstName.ToUpper(),
                    LastName = person.LastName.ToUpper(),
                    Gender = person.Gender,
                };

                //check if same firstname & lastname exist in the database before editting 
                var exist = await _peopleRepository.ExistPersonAsync(dbPerson);
                if (exist)
                {
                    response.Success = false;
                    response.Message = "You can not update person with an" +
                                        " existing person on the list";
                    return response;
                }
                //if the person exist delete the person
                var affected = await _peopleRepository.UpdatePersonAsync(dbPerson);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the person";
                    return response;
                }
            }
        }
    }
}
