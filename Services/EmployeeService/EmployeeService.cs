using DeviceSystem.Mapping;
using DeviceSystem.Repositories.EmployeeRespository;
using DeviceSystem.Requests.Employee;
using DeviceSystem.Services.AuthService;

namespace DeviceSystem.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRespository _employeeRespository;
        private readonly IAuthService _authService;

        // private readonly IDeviceLoanRepository _deviceLoanRepository;
        public EmployeeService(IEmployeeRespository employeeRespository,
         IAuthService authService)
        {
            _employeeRespository = employeeRespository;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> CreateEmployee(CreateEmployeeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to Employee Entity
            var employee = request.CreateMapToEmployee();
            //check if the Empoyee is already in the Database
            var exist = await _employeeRespository.ExistEmployeeAsync(employee);
            if (exist)
            {
                //if the employee is already in the database then return false and error message
                response.Success = false;
                response.Message = $"Employee already exist in the employees list.";
                return response;
            }
            else
            {
                //if the employee is not in the database then create a new one
                var affected = await _employeeRespository.CreateEmployeeAsync(employee);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to Enroll employee";
                    return response;
                }
            }
        }

        public Task<ServiceResponse<IEnumerable<DeviceLoanResponse>>> GetDeviceLoanById(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<EmployeeResponse>> GetEmployeeById(Guid employeeId)
        {
            var response = new ServiceResponse<EmployeeResponse>();
            // Get the employee from the database by Id
            var employee = await _employeeRespository.GetEmployeeByIdAsync(employeeId);

            if (employee is null)
            {
                //if the employee is not in the database then return false and error message
                response.Success = false;
                response.Message = "Employee not found.";
                return response;
            }
            else
            {
                //if the employee is in the database then map it to DTO 
                //and  return employee Response information
                response.Data = employee.MapToEmployeeResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<List<EmployeeResponse>>> GetEmployees()
        {
            var response = new ServiceResponse<List<EmployeeResponse>>();
            var _employeeList = new List<EmployeeResponse> ();

            var _employees = await _employeeRespository.GetEmployeesAsync();
            
            if (_employees.Count() == 0)
            {
                //if there are no employee in the database
                response.Success = false;
                response.Message = "There are no employees in the database yet.";
                return response;
            }
            else
            {
                //mapping all the employees to employee response
                _employees.ForEach(employee => {
                    _employeeList.Add(employee.MapToEmployeeResponse());
                });
                
                response.Data = _employeeList;
                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveEmployee(Guid employeeId)
        {
            var response = new ServiceResponse<bool>();
            //Get the employee from the database by id
            var employee = await _employeeRespository.GetEmployeeByIdAsync(employeeId);
            if (employee is null)
            {
                //if the employee is not in the database then return false and error message
                response.Success = false;
                response.Message = "Employee not found.";
                return response;
            }
            else
            {
                //employee.Department = null; employee.Person = null;
                //if them ployee exist delete the person
                var affected = await _employeeRespository.DeleteEmployeeAsync(employee);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the employee";
                    return response;
                }
            }
        }
        public async Task<ServiceResponse<bool>> UpdateEmployee(Guid employeeId, UpdateEmployeeRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to employee Entity
            var employee = request.UpdateMapToEmployee(employeeId);

            //get the employee from the database by its id
            var dbEmployee = await _employeeRespository.GetEmployeeByIdAsync(employeeId);
            if (dbEmployee is null)
            {
                //if the employee is not in the database then return false and error message
                response.Success = false;
                response.Message = "Employee not found.";
                return response;
            }
            else
            {
                dbEmployee = new Employee
                {
                    Id = dbEmployee.Id,
                    PersonId = dbEmployee.PersonId,
                    DepartmentId = employee.DepartmentId,
                    WorkEmail = employee.WorkEmail,
                    Enrollment = dbEmployee.Enrollment,
                };

                //check if same departmentId and personId exist in the database before editting 
                var exist = await _employeeRespository.ExistEmployeeAsync(dbEmployee);
                if (exist)
                {
                    response.Success = false;
                    response.Message = "You can not update employee with an" +
                                        " existing employee on the list";
                    return response;
                }
                else
                {
                    dbEmployee.Department = null; dbEmployee.Person = null;
                    //if the employee exist delete the employee
                    var affected = await _employeeRespository.UpdateEmployeeAsync(dbEmployee);
                    if (affected == 1)
                    {
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update the employee";
                        return response;
                    }
                }
            }
        }
    }
}