﻿using Microsoft.EntityFrameworkCore;

namespace DeviceSystem.Models
{
    [Keyless]
    public class DeviceSummary
    {
        public Guid DepartmentId { get; init; }
        public Guid DeviceTypeId {  get; init; }
        public string DepartmentName { get; init; } = string.Empty;
        public string TypeName { get; init; } = string.Empty;
        public string OfficeName { get; init; } = string.Empty;
        public int Total { get; init; }
        public int Unavailable { get; init; }
        public int Assigned { get; set; }
        public int Available
        {
            get
            {
                return (Total - Assigned) - Unavailable;
            }
        }
    }
}
