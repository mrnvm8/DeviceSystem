using DeviceSystem.Mapping;
using DeviceSystem.Repositories.DeviceRepository;
using DeviceSystem.Requests.Device;
using DeviceSystem.Services.AuthService;
using DeviceSystem.Services.DeviceLoanService;
using DeviceSystem.Services.EmployeeService;

namespace DeviceSystem.Services.DeviceService
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceLoanService _loanService;
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;
        public DeviceService(IDeviceRepository deviceRepository,
       IDeviceLoanService loanService, IAuthService authService,
       IEmployeeService employeeService)
        {
            _deviceRepository = deviceRepository;
            _loanService = loanService;
            _authService = authService;
            _employeeService = employeeService;
        }

        public async Task<ServiceResponse<DeviceResponse>> AddDevice(CreateDeviceRequest request)
        {
            var response = new ServiceResponse<DeviceResponse>();
            //mapping the request to device Entity
            var device = request.CreateMapToDevice();
            //check if the device is already in the Database
            var exist = await _deviceRepository.ExistAsync(device);
            if (exist)
            {
                //if the device is already in the database then return false and error message
                response.Success = false;
                response.Message = $"device already exist in the devices list.";
                return response;
            }
            else
            {
                //if the device is not in the database then create a new one
                var affected = await _deviceRepository.CreateAsync(device);
                if (affected == 1)
                {
                    response.Data = device.MapToDevice();
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create the device";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<DeviceResponse>> GetDeviceById(Guid deviceId)
        {
            var response = new ServiceResponse<DeviceResponse>();
            // Get the device from the database by Id
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device is null)
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
                var loan = new List<DeviceLoans>();
                response.Data = device.MapToDeviceResponse(new List<DeviceLoanResponse?>());
                return response;
            }
        }

        public async Task<ServiceResponse<List<DeviceResponse>>> GetDevices()
        {
            var response = new ServiceResponse<List<DeviceResponse>>();
            var _responseDevice = new List<DeviceResponse>();
            //Get the list of devices
            var _devices = new List<Device>();

            if (_authService.GetUserRole().Contains("Admin"))
            {
              _devices= await _deviceRepository.GetDevicesList();
            }
            else
            {
                //get user employeeId
                var userEmployeeId = _authService.GetEmployeeId();
                //first get the employee with the employeeId from user login
                var _employee = await _employeeService.GetEmployeeById(userEmployeeId);
                //get the personId from employee
                var _personId = _employee.Data!.PersonId;
                //get all employee
                var _employees = await _employeeService.GetEmployees();

                foreach (var employee in _employees.Data!.Where(x => x.PersonId.Equals(_personId)))
                {
                    var device =  await _deviceRepository.GetDevicesList();
                    _devices.AddRange(device.Where(x => x.DepartmentId.Equals(employee.DepartmentId)));
                }
            }

            if (_devices.Count() == 0)
            {
                //if there are no devices in the database
                response.Success = false;
                response.Message = "There are no devices in the database yet.";
                return response;
            }
            else
            {
                var _loan = await _loanService.GetDeviceLoans();
                var _LoanDevices = _loan.Data is null ? new List<DeviceLoanResponse>() : _loan.Data;
                _devices.ForEach(device =>
                {
                    _responseDevice.Add(device.MapToDeviceResponse(_LoanDevices!));
                });
                response.Data = _responseDevice;
                return response;
            };
        }

        public async Task<ServiceResponse<List<DeviceResponse>>> GetDevicesList(Guid departmentId, Guid DeviceTypeId)
        {
            var response = new ServiceResponse<List<DeviceResponse>>();
            var _deviceList = new List<DeviceResponse>();

            //Get the list of devices
            var _devices = await _deviceRepository.GetAllAsync(departmentId, DeviceTypeId);

            if (_devices is null)
            {
                //if there are no devices in the database
                response.Success = false;
                response.Message = "There are no devices in the database yet.";
                return response;
            }
            else
            {
                var _loan = await _loanService.GetDeviceLoans();
                var _LoanDevices = _loan.Data is null ? new List<DeviceLoanResponse>() : _loan.Data;
                _devices.ForEach(device =>
                {
                    _deviceList.Add(device.MapToDeviceResponse(_LoanDevices!));
                });
                response.Data = _deviceList;
                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveDevice(Guid deviceId)
        {
            var response = new ServiceResponse<bool>();
            //Get the device from the database by id
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device is null)
            {
                //if the device is not in the database then return false and error message
                response.Success = false;
                response.Message = "Person not found.";
                return response;
            }
            else
            {
                device.Department = null; device.DeviceType = null;
                //if the device exist delete the device
                var affected = await _deviceRepository.DeleteAsync(device);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the device";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<List<DeviceSummary>>> Summary()
        {
            var response = new ServiceResponse<List<DeviceSummary>>();
            var _deviceSummary = new List<DeviceSummary>();

            //Get the list of device summary


            if (_authService.GetUserRole().Contains("Admin"))
            {
                _deviceSummary = await _deviceRepository.DeviceSummary(Guid.Empty);
            }
            else
            {
                //get user employeeId
                var userEmployeeId = _authService.GetEmployeeId();
                //first get the employee with the employeeId from user login
                var _employee = await _employeeService.GetEmployeeById(userEmployeeId);
                //get the personId from employee
                var _personId = _employee.Data!.PersonId;
                //get all employee
                var _employees = await _employeeService.GetEmployees();

                foreach (var employee in _employees.Data!.Where(x => x.PersonId.Equals(_personId)))
                {
                    var _device = await _deviceRepository.DeviceSummary(employee.DepartmentId);
                    _deviceSummary.AddRange(_device);
                }
            }

            if (_deviceSummary.Any() == false)
            {
                //if there are no device in the database
                response.Success = false;
                response.Message = "There are no devices in the database yet.";
                return response;
            }
            else
            {
                response.Data = _deviceSummary;
                return response;
            };
        }

        public async Task<ServiceResponse<DeviceResponse>> UpdateDevice(Guid deviceId, UpdateDeviceRequest request)
        {
            var response = new ServiceResponse<DeviceResponse>();
            //mapping the request to device Entity
            var device = request.UpdateMapToDevice(deviceId);

            //get the device from the database by its id
            var dbDevice = await _deviceRepository.GetByIdAsync(deviceId);
            if (dbDevice is null)
            {
                //if the device is not in the database then return false and error message
                response.Success = false;
                response.Message = "device not found.";
                return response;
            }
            else
            {
                dbDevice = new Device
                {
                    Id = dbDevice.Id,
                    DepartmentId = device.DepartmentId,
                    DeviceTypeId = device.DeviceTypeId,
                    DeviceName = device.DeviceName,
                    Condition = device.Condition,
                    DeviceSerialNo = device.DeviceSerialNo,
                    DeviceIMEINo = device.DeviceIMEINo,
                    PurchasedPrice = device.PurchasedPrice,
                    PurchasedDate = device.PurchasedDate,
                };


                dbDevice.Department = null; dbDevice.DeviceType = null;
                //if the device exist delete the device
                var affected = await _deviceRepository.UpdateAsync(dbDevice);
                if (affected == 1)
                {
                    response.Data = dbDevice.MapToDevice();
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