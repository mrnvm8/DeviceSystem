using DeviceSystem.Requests.Device;

namespace DeviceSystem.Mapping
{
    public static class DeviceMapping
    {


        //This method is for mapping Device for creation
        public static Device CreateMapToDevice(this CreateDeviceRequest request)
        {
            return new Device
            {
                Id = Guid.NewGuid(),
                DeviceName = request.DeviceName,
                DepartmentId = request.DepartId,
                DeviceTypeId = request.TypeId,
                Condition = request.Condition,
                DeviceSerialNo = String.IsNullOrEmpty(request.SerialNo) ? "Null" : request.SerialNo,
                DeviceIMEINo = String.IsNullOrEmpty(request.IMEINo) ? "Null" : request.IMEINo,
                PurchasedPrice = request.PurchasedPrice,
                PurchasedDate = request.PurchasedDate,
                Year = request.PurchasedDate.Year,
            };
        }

        //This method is for mapping person to be updated
        public static Device UpdateMapToDevice(this UpdateDeviceRequest request, Guid deviceId)
        {
            return new Device
            {
                Id = deviceId,
                DeviceName = request.DeviceName,
                DepartmentId = request.DepartId,
                DeviceTypeId = request.TypeId,
                Condition = request.Condition,
                DeviceSerialNo = String.IsNullOrEmpty(request.SerialNo) ? "Null" : request.SerialNo,
                DeviceIMEINo = String.IsNullOrEmpty(request.IMEINo) ? "Null" : request.IMEINo,
                PurchasedPrice = request.PurchasedPrice,
                PurchasedDate = request.PurchasedDate,
                Year = request.PurchasedDate.Year,
            };
        }

        //This method is for mapping person to DTO
        public static DeviceResponse MapToDeviceResponse(this Device device, List<DeviceLoanResponse?> loans)
        {

            return new DeviceResponse
            {
                Id = device.Id,
                DeviceName = device.DeviceName,
                DepartId = device.DepartmentId,
                TypeId = device.DeviceTypeId,
                Condition = device.Condition,
                PurchasedPrice = device.PurchasedPrice,
                PurchasedDate = device.PurchasedDate,
                SerialNo = device.DeviceSerialNo == "Null" ? "" : device.DeviceSerialNo,
                IMEINo = device.DeviceIMEINo == "Null" ? "" : device.DeviceIMEINo,
                IdentityNumber = device.DeviceSerialNo != "Null" ?
                                   $"S/N: {device.DeviceSerialNo.ToUpper()}" : $"IMEI: {device.DeviceIMEINo.ToUpper()}",
                DepartmentName = device.Department?.DepartmentName,
                TypeName = device.DeviceType?.Name,
                FullName = (loans!.FirstOrDefault(x => x!.DeviceId.Equals(device.Id)))?.EmployeeName
            };
        }

         //This method is for mapping person to DTO
        public static DeviceResponse MapToDevice(this Device device)
        {

            return new DeviceResponse
            {
                Id = device.Id,
                DeviceName = device.DeviceName,
                DepartId = device.DepartmentId,
                TypeId = device.DeviceTypeId,
                Condition = device.Condition,
                PurchasedPrice = device.PurchasedPrice,
                PurchasedDate = device.PurchasedDate,
            };
        }
    }
}
