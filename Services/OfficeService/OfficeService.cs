using DeviceSystem.Mapping;
using DeviceSystem.Repositories.OfficeRepository;
using DeviceSystem.Requests.Office;

namespace DeviceSystem.Services.OfficeService
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        public OfficeService(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<ServiceResponse<bool>> CreateOffice(CreateOfficeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to office Entity
            var office = request.CreateMapToOffice();
            //check if the office is already in the Database
            var exist = await _officeRepository.ExistAsync(office);
            if (exist)
            {
                //if the office is already in the database then return false and error message
                response.Success = false;
                response.Message = $"Office Name already exist in the Office list.";
                return response;
            }
            else
            {
                //if the office is not in the database then create a new one
                var affected = await _officeRepository.CreateAsync(office);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create the Office";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<OfficeResponse>> GetOfficeById(Guid officeId)
        {
            var response = new ServiceResponse<OfficeResponse>();
            // Get the office from the database by Id
            var office = await _officeRepository.GetByIdAsync(officeId);

            if (office is null)
            {
                //if the person is not in the database then return false and error message
                response.Success = false;
                response.Message = "Office not found.";
                return response;
            }
            else
            {
                //if the office is in the database then map it to DTO 
                //and  return office Dto information
                response.Data = office.MapToOfficeResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<IEnumerable<OfficeResponse>>> GetOfficesList()
        {
            var response = new ServiceResponse<IEnumerable<OfficeResponse>>();
            //Get the list of office
            var offices = await _officeRepository.GetAllAsync();

            if (offices.Any() == false)
            {
                //if there are no people in the database
                response.Success = false;
                response.Message = "There are no offices in the database yet.";
                return response;
            }
            else
            {
                response.Data = offices.Select(x => x!.MapToOfficeResponse());
                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveOffice(Guid officeId)
        {
            var response = new ServiceResponse<bool>();
            //Get the office from the database by id
            var office = await _officeRepository.GetByIdAsync(officeId);
            if (office is null)
            {
                //if the office is not in the database then return false and error message
                response.Success = false;
                response.Message = "Office not found.";
                return response;
            }
            else
            {
                //if the office exist delete the person
                var affected = await _officeRepository.DeleteAsync(office);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the office";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> UpdateOffice(Guid officeId, UpdateOfficeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to office Entity
            var office = request.UpdateMapToOffice(officeId);

            //get the office from the database by its id
            var dbOffice = await _officeRepository.GetByIdAsync(officeId);
            if (dbOffice is null)
            {
                //if the office is not in the database then return false and error message
                response.Success = false;
                response.Message = "Office not found.";
                return response;
            }
            else
            {
                dbOffice = new Office
                {
                    Id = dbOffice.Id,
                    Name = request.Name.ToUpper(),
                    Location = request.Location.ToUpper(),
                };

                //check if same firstname & lastname exist in the database before editting 
                var exist = await _officeRepository.ExistAsync(dbOffice);
                if (exist)
                {
                    response.Success = false;
                    response.Message = "You can not update office with an" +
                                        " existing office on the list";
                    return response;
                }
                //if the office exist delete the person
                var affected = await _officeRepository.UpdateAsync(dbOffice);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the office";
                    return response;
                }
            }
        }
    }
}
