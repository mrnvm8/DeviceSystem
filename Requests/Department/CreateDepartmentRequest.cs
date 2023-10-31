﻿namespace DeviceSystem.Requests.Department
{
    public class CreateDepartmentRequest
    {
        public Guid OfficeId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
