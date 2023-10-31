namespace DeviceSystem.Repositories.DeviceTypeRepository
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        private readonly ApplicationContext _context;

        public DeviceTypeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(DeviceType deviceType)
        {
            // add the device Type
            _context.Attach(deviceType);
            _context.Entry(deviceType).State = EntityState.Added;
            //getting the status if any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> DeleteAsync(DeviceType deviceType)
        {
            // remove the device Type
            _context.Attach(deviceType);
            _context.Entry(deviceType).State = EntityState.Deleted;
            //getting the status if any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<bool> ExistAsync(DeviceType deviceType)
        {
            //checking if the employee exist with same department
            //from the database return true/false;
            return await _context.DeviceTypes!
                     .AsNoTracking()
                     .AnyAsync(x => x.Name.Equals(deviceType.Name) && 
                     x.Description.Equals(deviceType.Description));
        }

        public async Task<IEnumerable<DeviceType?>> GetAllAsync() =>
            await _context.DeviceTypes!.AsNoTracking().ToListAsync();

        public async Task<DeviceType?> GetByIdAsync(Guid id) =>
            await _context.DeviceTypes!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> UpdateAsync(DeviceType deviceType)
        {
            //update the device Type
            _context.Attach(deviceType);
            _context.Entry(deviceType).State = EntityState.Modified;
            //getting the status if any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }
    }
}
