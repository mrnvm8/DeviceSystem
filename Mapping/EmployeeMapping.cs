using DeviceSystem.Requests.Employee;

namespace DeviceSystem.Mapping
{
    public static class EmployeeMapping
    {
        //This method is for mapping person for creation
        public static Employee CreateMapToEmployee(this CreateEmployeeRequest request)
        {
            return new Employee
            {
                Id = Guid.NewGuid(),
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                WorkEmail = request.WorkEmail.ToLower(),
                Enrollment = DateTime.Now,
                IsEmployeeActive = true
            };
        }

        //This method is for mapping person for creation
        public static Employee UpdateMapToEmployee(this UpdateEmployeeRequest request, Guid employeeId)
        {
            return new Employee
            {
                Id = employeeId,
                PersonId = request.PersonId,
                DepartmentId = request.DepartmentId,
                WorkEmail = request.WorkEmail.ToLower(),
                Enrollment = request.Enrollment,
                IsEmployeeActive = request.IsEmployeeActive
            };
        }

        //This method is for mapping employee to employeeResponse
        public static EmployeeResponse MapToEmployeeResponse(this Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                PersonId = employee.PersonId,
                DepartmentId = employee.DepartmentId,
                WorkEmail = employee.WorkEmail.ToLower(),
                Enrollment = employee.Enrollment,
                FullName = $"{employee.Person?.FirstName}," +
                           $" {employee.Person?.LastName}",
                DepartmentName = employee.Department?.DepartmentName,
                OfficeName = employee.Department?.Offices?.Name
            };
        }
    }
}
