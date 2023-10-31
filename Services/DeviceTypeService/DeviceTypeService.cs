using DeviceSystem.Mapping;
using DeviceSystem.Repositories.DeviceTypeRepository;
using DeviceSystem.Requests.DeviceType;

namespace DeviceSystem.Services.DeviceTypeService
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IDeviceTypeRepository _typeService;

        public DeviceTypeService(IDeviceTypeRepository typeService)
        {
            _typeService = typeService;
        }

        public async Task<ServiceResponse<bool>> AddDeviceType(CreateTypeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to device type Entity
            var devicetype = request.CreateMapToDeviceType();
            //check if the device type is already in the Database
            var exist = await _typeService.ExistAsync(devicetype);
            if (exist)
            {
                //if the device type is already in the database then return false and error message
                response.Success = false;
                response.Message = $"device type already exist in the device types list.";
                return response;
            }
            else
            {
                //if the device type is not in the database then create a new one
                var affected = await _typeService.CreateAsync(devicetype);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create the device type";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<DeviceTypeResponse>> GetDeviceTypeById(Guid typeId)
        {
            var response = new ServiceResponse<DeviceTypeResponse>();
            // Get the device type from the database by Id
            var devicetype = await _typeService.GetByIdAsync(typeId);

            if (devicetype is null)
            {
                //if the type is not in the database then return false and error message
                response.Success = false;
                response.Message = "device type not found.";
                return response;
            }
            else
            {
                //if the device type is in the database then map it to DTO 
                //and  return device type Dto information
                response.Data = devicetype.MapToDeviceTypeResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<DeviceTypeResponse>>> GetDeviceTypesList()
        {
            var response = new ServiceResponse<IEnumerable<DeviceTypeResponse>>();
            //Get the list of device types
            var deviceTypes = await _typeService.GetAllAsync();

            if (deviceTypes.Any() == false)
            {
                //if there are no device types in the database
                response.Success = false;
                response.Message = "There are no device types in the database yet.";
                return response;
            }
            else
            {
                response.Data = deviceTypes.Select(x => x!.MapToDeviceTypeResponse());
                return response;
            };
        }

        public async Task<ServiceResponse<bool>> RemoveDeviceType(Guid typeId)
        {
            var response = new ServiceResponse<bool>();
            //Get the device type from the database by id
            var deviceType = await _typeService.GetByIdAsync(typeId);
            if (deviceType is null)
            {
                //if the device type is not in the database then return false and error message
                response.Success = false;
                response.Message = "device type not found.";
                return response;
            }
            else
            {
                //if the device type exist delete the device
                var affected = await _typeService.DeleteAsync(deviceType);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the device type";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> UpdateDeviceType(Guid typeId, UpdateTypeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to device Entity
            var deviceType = request.UpdateMapToDeviceType(typeId);
            //get the device type from the database by its id
            var dbDeviceType = await _typeService.GetByIdAsync(typeId);
            if (dbDeviceType is null)
            {
                //if the device type is not in the database then return false and error message
                response.Success = false;
                response.Message = "device type not found.";
                return response;
            }
            else
            {
                // dbDeviceType = dbDeviceType with
                // {
                //     Name = request.Name,
                //     Description = request.Description,
                // };

                //check if same device type exist in the database before editting 
                var exist = await _typeService.ExistAsync(dbDeviceType);
                if (exist)
                {
                    response.Success = false;
                    response.Message = "You can not update device type with an" +
                                        " existing device type on the list";
                    return response;
                }
                //if the device type exist delete the device
                var affected = await _typeService.UpdateAsync(dbDeviceType);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the device type";
                    return response;
                }
            }
        }
    }
}
