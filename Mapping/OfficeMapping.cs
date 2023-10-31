using DeviceSystem.Requests.Office;

namespace DeviceSystem.Mapping
{
    public static class OfficeMapping
    {
        //This method is for mapping office for creation
        public static Office CreateMapToOffice(this CreateOfficeRequest request)
        {
            return new Office
            {
                Id = Guid.NewGuid(),
                Name = request.Name.ToUpper(),
                Location = request.Location.ToUpper()
            };
        }

        //This method is for mapping Office for to be updated
        public static Office UpdateMapToOffice(this UpdateOfficeRequest request, Guid officeId)
        {
            return new Office
            {
                Id = officeId,
                Name = request.Name,
                Location = request.Location
            };
        }

        //This method is for mapping Office to DTO
        public static OfficeResponse MapToOfficeResponse(this Office office)
        {
            return new OfficeResponse
            {
                Id = office.Id,
                Name = office.Name,
                Location = office.Location
            };
        }
    }
}
