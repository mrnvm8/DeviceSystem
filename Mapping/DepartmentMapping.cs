using DeviceSystem.Requests.Department;

namespace DeviceSystem.Mapping
{
    public static class DepartmentMapping
    {
        //This method is for mapping department for creation
        public static Department CreateMapToDepartment(this CreateDepartmentRequest request)
        {
            return new Department
            {
                Id = Guid.NewGuid(),
                OfficeId = request.OfficeId,
                DepartmentName = request.DepartmentName.ToUpper(),
                Description = request.Description,
            };
        }

        //This method is for mapping department for creation
        public static Department UpdateMapToDepartment(this UpdateDepartmentRequest request, Guid departmentId)
        {
            return new Department
            {
                Id = departmentId,
                OfficeId = request.OfficeId,
                DepartmentName = request.DepartmentName.ToUpper(),
                Description = request.Description,
            };
        }

        //This method is for mapping derson to DTO
        public static DepartmentResponse MapToDepartmentResponse(this Department department)
        {
            return new DepartmentResponse
            {
                Id = department.Id,
                OfficeId = department.OfficeId,
                DepartmentName = department.DepartmentName,
                Description = department.Description,
                OfficeName = department.Offices!.Name
            };
        }
    }
}
