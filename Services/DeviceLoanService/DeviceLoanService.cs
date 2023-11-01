using DeviceSystem.Mapping;
using DeviceSystem.Repositories.DeviceLoanRepository;
using DeviceSystem.Requests.DeviceLoans;

namespace DeviceSystem.Services.DeviceLoanService
{
    public class DeviceLoanService : IDeviceLoanService
    {
        private readonly IDeviceLoanRepository _deviceLoanRepository;
        
        public DeviceLoanService(IDeviceLoanRepository deviceLoanRepository)
        {
            _deviceLoanRepository = deviceLoanRepository;
        }

        public async Task<ServiceResponse<DeviceLoanResponse>> AssignDevice(CreateDeviceLoanRequest request)
        {
            var response = new ServiceResponse<DeviceLoanResponse>();
            //mapping the request to device loan Entity
            var loan = request.CreateMapToDeviceLoan();
            //if the device is not in the database then create a new one
            //loan.Device = null; loan.Employee = null;
            var affected = await _deviceLoanRepository.CreateAsync(loan);
            if (affected == 1)
            {
                //response.Data = loan.MapToDeviceLoanResponse();
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to assign the device";
                return response;
            }
        }
        public async Task<ServiceResponse<DeviceLoanResponse>> GetDeviceLoanById(Guid deviceLoanId)
        {
            var response = new ServiceResponse<DeviceLoanResponse>();
            // Get the device from the database by Id
            var deviceLoan = await _deviceLoanRepository.GetByIdAsync(deviceLoanId);

            if (deviceLoan is null)
            {
                //if the device is not in the database then return false and error message
                response.Success = false;
                response.Message = "device not found.";
                return response;
            }
            else
            {
                //if the device is in the database then map it to DTO 
                //and  return device Dto information
                response.Data = deviceLoan.MapToDeviceLoanResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<List<DeviceLoanResponse>>> GetDeviceLoans()
        {
            var response = new ServiceResponse<List<DeviceLoanResponse>>();
            var _AssignDevice = new List<DeviceLoanResponse>();
            //Get the list of devices loan
            var deviceloans = await _deviceLoanRepository.GetAllAsync();

            if (deviceloans.Count() == 0)
            {
                //if there are no devices loan in the database
                response.Success = false;
                response.Message = "There are no device loans in the database yet.";
                return response;
            }
            else
            {
                //getting the list and map each item to response
                deviceloans.ForEach(loan =>{
                    _AssignDevice.Add(loan.MapToDeviceLoanResponse());
                });

                response.Data = _AssignDevice;
                return response;
            };
        }

        public async Task<ServiceResponse<List<DeviceLoanResponse>>> GetAllDeviceLoansById(Guid deviceId)
        {
            var response = new ServiceResponse<List<DeviceLoanResponse>>();
            var _AssignDevice = new List<DeviceLoanResponse>();
            // Get the device loan from the database by Id
            var deviceLoans = await _deviceLoanRepository.GetDeviceLoansByDeviceId(deviceId);

            if (deviceLoans is null)
            {
                //if the device loan is not in the database then return false and error message
                response.Success = false;
                response.Message = "devices not found.";
                return response;
            }
            else
            {
                //if the device loan is in the database then map it to DTO 
                //and  return device loan Dto information
                //getting the list and map each item to response
                deviceLoans.ForEach(loan =>
                {
                    _AssignDevice.Add(loan.MapToDeviceLoanResponse());
                });

                response.Data = _AssignDevice;
                return response;
            }
        }

        public async Task<ServiceResponse<DeviceLoanResponse>> UnassignedDevice(Guid deviceLoanId, UpdateDeviceLoanRequest request)
        {
            var response = new ServiceResponse<DeviceLoanResponse>();
            //mapping the request to device loan Entity
            var deviceLoan = request.UpdateMapToDeviceLoan(deviceLoanId);

            //get the device loan from the database by its id
            var dbDeviceLoan = await _deviceLoanRepository.GetByIdAsync(deviceLoanId);
            if (dbDeviceLoan is null)
            {
                //if the device is not in the database then return false and error message
                response.Success = false;
                response.Message = "device loan not found.";
                return response;
            }
            else
            {
                dbDeviceLoan = new DeviceLoans
                {
                    Id = dbDeviceLoan.Id,
                    UserId = dbDeviceLoan.UserId,
                    DeviceId = dbDeviceLoan.DeviceId,
                    EmployeeId = dbDeviceLoan.EmployeeId,
                    AssignedDate = dbDeviceLoan.AssignedDate,
                    IsApproved = true,
                    ReturnToUserId = deviceLoan.UserId,
                    ReturnDate = deviceLoan.ReturnDate
                };

                //dbDeviceLoan.Device = null; dbDeviceLoan.Employee = null;
                //if the device exist delete the device
                var affected = await _deviceLoanRepository.UpdateAsync(dbDeviceLoan);
                if (affected == 1)
                {
                    //response.Data = deviceLoan.MapToDeviceLoanResponse();
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the device";
                    return response;
                }
            }
        }
    }
}
