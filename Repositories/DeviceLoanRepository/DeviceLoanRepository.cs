namespace DeviceSystem.Repositories.DeviceLoanRepository
{
    public class DeviceLoanRepository : IDeviceLoanRepository
    {
        private readonly ApplicationContext _context;

        public DeviceLoanRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(DeviceLoans deviceLoans)
        {
            //attach device loan entity
            _context.Attach(deviceLoans);
            //Adding device loan to the database
            _context.Entry(deviceLoans).State = EntityState.Added;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<DeviceLoans?> GetByIdAsync(Guid deviceId) =>
            await _context.DeviceLoans!
            .Include(d => d.Device)
            .Include(e => e.Employee)
               .ThenInclude(p => p!.Person)
            .Where(x => x.ReturnDate.Equals(DateTime.Parse("2009-01-01")))
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DeviceId.Equals(deviceId));

        public async Task<List<DeviceLoans>> GetAllAsync() => 
           await _context.DeviceLoans!
           .Include(d => d.Device)
                .ThenInclude(t => t!.DeviceType)
           .Include(e => e.Employee)
              .ThenInclude(p => p!.Person)
           .Where(x => x.ReturnDate.Equals(DateTime.Parse("2009-01-01")))
           .AsNoTracking()
           .ToListAsync();

        public async Task<int> UpdateAsync(DeviceLoans deviceLoans)
        {
            //attach device loan entity
            _context.Attach(deviceLoans);
            //Adding device loan to the database
            _context.Entry(deviceLoans).State = EntityState.Modified;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<List<DeviceLoans>> GetDeviceLoansByDeviceId(Guid deviceId) => 
            await _context.DeviceLoans!
           .Include(d => d.Device)
                .ThenInclude(t => t!.DeviceType)
           .Include(e => e.Employee)
              .ThenInclude(p => p!.Person)
            .Where(x => x.DeviceId.Equals(deviceId))
           .AsNoTracking()
           .ToListAsync();

        public async Task<List<DeviceLoans>> GetDeviceLoansByEmployeeId(Guid employeeId)=>
            await _context.DeviceLoans!
            .Include(d => d.Device)
                .ThenInclude(t => t!.DeviceType)
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Person)
            .Where(x => x.EmployeeId.Equals(employeeId) &&
             x.ReturnDate.Equals(DateTime.Parse("2009-01-01")))
            .AsNoTracking()
            .ToListAsync();
        }
}
