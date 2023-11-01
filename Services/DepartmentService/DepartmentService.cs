using DeviceSystem.Mapping;
using DeviceSystem.Repositories.DepartmentRepository;
using DeviceSystem.Repositories.EmployeeRespository;
using DeviceSystem.Requests.Department;
using DeviceSystem.Services.AuthService;

namespace DeviceSystem.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAuthService _authService;
        private readonly IEmployeeRespository _employeeRespository;

        public DepartmentService(IDepartmentRepository departmentRepository,
        IAuthService authService, 
        IEmployeeRespository employeeRespository)
        {
            _departmentRepository = departmentRepository;
            _authService = authService;
            _employeeRespository = employeeRespository;
        }

        public async Task<ServiceResponse<bool>> CreateDepartment(CreateDepartmentRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to department Entity
            var department = request.CreateMapToDepartment();
            //check if the department is already in the Database
            var exist = await _departmentRepository.ExistAsync(department);
            if (exist)
            {
                //if the department is already in the database then return false and error message
                response.Success = false;
                response.Message = $"Employee already exist in the employees list.";
                return response;
            }
            else
            {
                //if the department is not in the database then create a new one
                var affected = await _departmentRepository.CreateAsync(department);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create department";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<DepartmentResponse>> GetDepartmentById(Guid departmentId)
        {
            var response = new ServiceResponse<DepartmentResponse>();
            // Get the department from the database by Id
            var department = await _departmentRepository.GetByIdAsync(departmentId);

            if (department is null)
            {
                //if the department is not in the database then return false and error message
                response.Success = false;
                response.Message = "department not found.";
                return response;
            }
            else
            {
                //if the department is in the database then map it to DTO 
                //and  return department Response information
                response.Data = department.MapToDepartmentResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<List<DepartmentResponse>>> GetDepartmentsList()
        {
            var response = new ServiceResponse<List<DepartmentResponse>>();
            var _departmentList = new List<DepartmentResponse>();
            //Get the list of departments
            var departments = new List<Department>();

            if (_authService.GetUserRole().Contains("Admin"))
            {
                departments = await _departmentRepository.GetAllAsync();

            }
            else
            {
                //! I don't like this at all, this can be made into a function since it will 
                //! repeat in the code some where
                //? it does the job but not pretty i must say. please think for something for V2 
                //TODO: Either I get rid of the person table and combine 
                //TODO: the information in the employee table. 
                
                //get user employeeId
                var userEmployeeId = _authService.GetEmployeeId();
                //first get the employee with the employeeId from user login
                var _employee = await _employeeRespository.GetEmployeeByIdAsync(userEmployeeId);
                //get the personId from employee
                var _personId = _employee!.PersonId;
                //get all employee
                var _employees = await _employeeRespository.GetEmployeesAsync();

                foreach (var employee in _employees.Where(x => x.PersonId.Equals(_personId)))
                {
                    var _departments = await _departmentRepository.GetAllAsync();
                    departments.AddRange(_departments.Where(x => x.Id.Equals(employee.DepartmentId)));

                }
            }

            if (departments.Count() == 0)
            {
                //if there are nodepartment in the database
                response.Success = false;
                response.Message = "There are no departments in the database yet.";
                return response;
            }
            else
            {
                departments.ForEach(depart =>
                {
                    _departmentList.Add(depart.MapToDepartmentResponse());
                });
                response.Data = _departmentList;
                return response;
            }
        }

        public async Task<ServiceResponse<bool>> RemoveDepartment(Guid departmentId)
        {
            var response = new ServiceResponse<bool>();
            //Get the department from the database by id
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department is null)
            {
                //if the department is not in the database then return false and error message
                response.Success = false;
                response.Message = "department not found.";
                return response;
            }
            else
            {
                //department.Offices = null;
                //if them department exist delete the person
                var affected = await _departmentRepository.DeleteAsync(department);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the department";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> UpdateDepartment(Guid departmentId, UpdateDepartmentRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to department Entity
            var department = request.UpdateMapToDepartment(departmentId);
            //get the department from the database by its id
            var dbDepartment = await _departmentRepository.GetByIdAsync(departmentId);
            if (dbDepartment is null)
            {
                //if the department is not in the database then return false and error message
                response.Success = false;
                response.Message = "department not found.";
                return response;
            }
            else
            {
                dbDepartment = new Department
                {
                    Id = dbDepartment.Id,
                    DepartmentName = department.DepartmentName,
                    OfficeId = department.OfficeId,
                    Description = department.Description
                };

                //check if same department Id and personId exist in the database before editting 
                var exist = await _departmentRepository.ExistAsync(dbDepartment);
                if (exist)
                {
                    response.Success = false;
                    response.Message = "You can not update department with an" +
                                        " existing department on the list";
                    return response;
                }
                else
                {
                    dbDepartment.Offices = null;
                    //if the employee exist delete the department
                    var affected = await _departmentRepository.UpdateAsync(dbDepartment);
                    if (affected == 1)
                    {
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update the department";
                        return response;
                    }
                }

            }
        }
    }
}
