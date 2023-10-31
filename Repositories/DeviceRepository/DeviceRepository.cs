namespace DeviceSystem.Repositories.DeviceRepository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationContext _context;

        public DeviceRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetDevicesList() =>
           await _context.Devices!
           .Include(d => d.Department)
               .ThenInclude(o => o!.Offices)
           .Include(d => d.DeviceType)
           .AsNoTracking()
           .ToListAsync();

        public async Task<List<Device>> GetAllAsync(Guid departmentId, Guid DeviceTypeId) => 
            await _context.Devices!
            .Include(d => d.Department)
                .ThenInclude(o => o!.Offices)
            .Include(d => d.DeviceType)
            .Where(x => x.DepartmentId.Equals(departmentId) 
            && x.DeviceTypeId.Equals(DeviceTypeId))
            .AsNoTracking()
            .ToListAsync();

        public async Task<Device?> GetByIdAsync(Guid id) => 
            await _context.Devices!
            .Include(d => d.Department)
                .ThenInclude(o => o!.Offices)
            .Include(d => d.DeviceType)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<int> CreateAsync(Device device)
        {
            //Adding device to the database
            _context.Attach(device);
            _context.Entry(device).State = EntityState.Added;
            //getting the status if  any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> DeleteAsync(Device device)
        {
            //remove the device
            _context.Attach(device);
            _context.Entry(device).State = EntityState.Deleted;
            //getting the status if any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }
        public async Task<int> UpdateAsync(Device device)
        {
            //updating the device
            _context.Attach(device);
            _context.Entry(device).State = EntityState.Modified;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<bool> ExistAsync(Device device)
        {

            //checking if the device exist with same department
            //from the database return true/false;
            var exist = false;
            if (device.DeviceSerialNo == "Null")
            {
                exist = await _context.Devices!
                     .AsNoTracking()
                     .AnyAsync(x => x.DeviceName.Equals(device.DeviceName)
                      && x.DeviceTypeId.Equals(device.DeviceTypeId)
                      && x.DeviceIMEINo.Equals(device.DeviceIMEINo));
            }
            else if(device.DeviceIMEINo == "Null")
            {
                exist = await _context.Devices!
                     .AsNoTracking()
                     .AnyAsync(x => x.DeviceName.Equals(device.DeviceName)
                      && x.DeviceTypeId.Equals(device.DeviceTypeId)
                      && x.DeviceSerialNo.Equals(device.DeviceSerialNo));
            }
            //else{
            //    //This condition is for when someone put serial number in the IMEI number and they exist
            //    //basically swapping the device identification number to trixk the system
            //    //someome can can how to write the name, they can not change the type and identity number of device
            //    exist = await _context.Devices!
            //         .AsNoTracking()
            //         .AnyAsync(x =>x.DeviceName != device.DeviceName
            //         && x.DeviceSerialNo.Equals(device.DeviceIMEINo) 
            //         || x.DeviceIMEINo.Equals(device.DeviceSerialNo));
            //}
            return exist;
            
        }

        public async Task<List<DeviceSummary>> DeviceSummary(Guid departmentId)
        {
            var date = Convert.ToDateTime("2009-01-01").ToString("yyyy-MM-dd");
            var sql = "";

            if(departmentId == Guid.Empty)
            {
                 sql = String.Format("SELECT d.DeviceTypeId, p.Id AS DepartmentId, p.DepartmentName, "+
                    "t.Name AS TypeName, o.Name AS OfficeName, COUNT(*) AS Total, " +
                    "(SELECT COUNT(*) FROM DeviceLoans l  " +
                    "LEFT JOIN Devices e ON l.DeviceId = e.Id " +
                    "LEFT JOIN Departments s ON e.DepartmentId = s.Id " +
                    "WHERE e.DeviceTypeId = d.DeviceTypeId AND e.DepartmentId = d.DepartmentId " +
                    "AND l.ReturnDate = '{0}') AS Assigned, " +
                    "(SELECT COUNT(*) FROM Devices u " +
                    "LEFT JOIN Departments p ON u.DepartmentId = p.Id " +
                    "WHERE u.DeviceTypeId = d.DeviceTypeId AND u.DepartmentId = d.DepartmentId " +
                    "AND(u.Condition = '3' OR u.Condition = '4' OR u.Condition = '5' OR u.Condition = '6')) AS Unavailable " +
                    "FROM Devices d " +
                    "LEFT JOIN Departments p ON p.Id = d.DepartmentId " +
                    "LEFT JOIN DeviceTypes t ON t.Id = d.DeviceTypeId " +
                    "LEFT JOIN Offices o ON o.Id = p.OfficeId " +
                    "GROUP BY d.DeviceTypeId, d.DepartmentId", date);
            }else{

                sql = String.Format("SELECT d.DeviceTypeId, p.Id AS DepartmentId, p.DepartmentName, " +
                    "t.Name AS TypeName, o.Name AS OfficeName, COUNT(*) AS Total, " +
                    "(SELECT COUNT(*) FROM DeviceLoans l  " +
                    "LEFT JOIN Devices e ON l.DeviceId = e.Id " +
                    "LEFT JOIN Departments s ON e.DepartmentId = s.Id " +
                    "WHERE e.DeviceTypeId = d.DeviceTypeId AND e.DepartmentId = d.DepartmentId " +
                    "AND l.ReturnDate = '{0}') AS Assigned, " +
                    "(SELECT COUNT(*) FROM Devices u " +
                    "LEFT JOIN Departments p ON u.DepartmentId = p.Id " +
                    "WHERE u.DeviceTypeId = d.DeviceTypeId AND u.DepartmentId = d.DepartmentId " +
                    "AND(u.Condition = '3' OR u.Condition = '4' OR u.Condition = '5' OR u.Condition = '6')) AS Unavailable " +
                    "FROM Devices d " +
                    "LEFT JOIN Departments p ON p.Id = d.DepartmentId " +
                    "LEFT JOIN DeviceTypes t ON t.Id = d.DeviceTypeId " +
                    "LEFT JOIN Offices o ON o.Id = p.OfficeId " +
                    "WHERE d.DepartmentId = '{1}' " +
                    "GROUP BY d.DeviceTypeId, d.DepartmentId", date, departmentId);
            }
            
            var result = await _context.DevicesSummaries!
                .FromSqlRaw(sql)
                .ToListAsync();

            return result;
        }

    }
}
