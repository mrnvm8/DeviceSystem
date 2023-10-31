using DeviceSystem.Requests.DeviceType;

namespace DeviceSystem.Mapping
{
    public static class DeviceTypeMapping
    {
        //This method is for mapping DeviceType for creation
        public static DeviceType CreateMapToDeviceType(this CreateTypeRequest request)
        {
            return new DeviceType
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
            };
        }

        //This method is for mapping DeviceType to be updated
        public static DeviceType UpdateMapToDeviceType(this UpdateTypeRequest request, Guid TypeId)
        {
            return new DeviceType
            {
                Id = TypeId,
                Name = request.Name,
                Description = request.Description
            };
        }

        //This method is for mapping DeviceTypeResponse to DTO
        public static DeviceTypeResponse MapToDeviceTypeResponse(this DeviceType deviceType)
        {
            return new DeviceTypeResponse
            {
                Id = deviceType.Id,
                Name = deviceType.Name,
                Description = deviceType.Description
            };
        }
    }
}
