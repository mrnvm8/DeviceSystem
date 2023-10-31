using DeviceSystem.Requests.DeviceLoans;

namespace DeviceSystem.Mapping
{
    public static class DeviceLoanMapping
    {
        //This method is for mapping Device for creation
        public static DeviceLoans CreateMapToDeviceLoan(this CreateDeviceLoanRequest request)
        {
            return new DeviceLoans
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DeviceId = request.DeviceId,
                EmployeeId = request.EmployeeId,
                AssignedDate = DateTime.Now,
                ReturnDate = DateTime.Parse("2009-01-01"),
                IsApproved = false,
            };
        }

        //This method is for mapping person to be updated
        public static DeviceLoans UpdateMapToDeviceLoan(this UpdateDeviceLoanRequest request, Guid deviceLoanId)
        {
            return new DeviceLoans
            {
                Id = deviceLoanId,
                UserId = request.UserId,
                DeviceId = request.DeviceId,
                EmployeeId = request.EmployeeId,
                ReturnDate = DateTime.Now,
                ReturnToUserId = request.UserId,
                IsApproved = true,
            };
        }

        //This method is for mapping person to DTO
        public static DeviceLoanResponse MapToDeviceLoanResponse(this DeviceLoans deviceLoans)
        {
            return new DeviceLoanResponse
            {
                Id = deviceLoans.Id,
                DeviceId = deviceLoans.DeviceId,
                DepartmentId = deviceLoans.Device!.DepartmentId,
                DeviceTypeId = deviceLoans.Device!.DeviceTypeId,
                EmployeeId = deviceLoans.EmployeeId,
                AssignedDate = deviceLoans.AssignedDate,
                ReturnDate = deviceLoans.ReturnDate,
                IsApproved = deviceLoans.IsApproved,
                EmployeeName = $"{deviceLoans!.Employee?.Person?.FirstName}, " +
                              $"{deviceLoans!.Employee?.Person?.LastName}",
                LoanDevice = deviceLoans.Device
            };
        }
    }
}
