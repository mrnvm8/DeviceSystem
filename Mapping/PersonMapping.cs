using DeviceSystem.Requests.Person;

namespace DeviceSystem.Mapping
{
    public static class PersonMapping
    {
        //This method is for mapping person for creation
        public static Person CreateMapToPerson(this CreatePersonRequest request)
        {
            return new Person
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName.ToUpper(),
                LastName = request.LastName.ToUpper(),
                Gender = request.Gender
            };
        }

        //This method is for mapping person to be updated
        public static Person UpdateMapToPerson(this UpdatePersonRequest request, Guid personId)
        {
            return new Person
            {
                Id = personId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender
            };
        }

        //This method is for mapping person to DTO
        public static PersonResponse MapToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender
            };
        }
    }
}
