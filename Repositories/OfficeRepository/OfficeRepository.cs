namespace DeviceSystem.Repositories.OfficeRepository
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly ApplicationContext _context;
        public OfficeRepository(ApplicationContext context) {
            _context = context;
        }

        //Get list of office
        public async Task<IEnumerable<Office?>> GetAllAsync() => await _context.Offices!
                                                                .AsNoTracking()
                                                                .ToListAsync();
        //getting the office by id
        public async Task<Office?> GetByIdAsync(Guid id) => await _context.Offices!
                                                            .AsNoTracking()
                                                            .FirstOrDefaultAsync(o => o.Id == id);
        public async Task<int> CreateAsync(Office office)
        {
            //attach office entity
            _context.Attach(office);
            //Adding office to the database
            _context.Entry(office).State = EntityState.Added;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<int> DeleteAsync(Office office)
        {
            //attach office entity
            _context.Attach(office);
            //Adding office to the database
            _context.Entry(office).State = EntityState.Deleted;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<int> UpdateAsync(Office office)
        {
            //attach office entity
            _context.Attach(office);
            //Addingoffice to the database
            _context.Entry(office).State = EntityState.Modified;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }
        public async Task<bool> ExistAsync(Office office)
        {
            //checking if the office exist from the database return true/false;
            return await _context.Offices!
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower().Equals(office.Name.ToLower()) 
                && x.Location.ToLower().Equals(office.Location.ToLower()));
        }

       

       
    }
}
