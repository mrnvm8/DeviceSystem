namespace DeviceSystem.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationContext _context;

        public DepartmentRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync() =>
         await _context.Departments!
                .Include(o => o.Offices)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Department?> GetByIdAsync(Guid id) =>
            await _context.Departments!
                .Include(o => o.Offices)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<int> CreateAsync(Department department)
        {
            //attach department entity
            _context.Attach(department);
            //Adding department to the database
            _context.Entry(department).State = EntityState.Added;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<int> DeleteAsync(Department department)
        {
            //attach department entity
            _context.Attach(department);
            //Adding department to the database
            _context.Entry(department).State = EntityState.Deleted;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<int> UpdateAsync(Department department)
        {
            //attach department entity
            _context.Attach(department);
            //Adding department to the database
            _context.Entry(department).State = EntityState.Modified;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }
        public async Task<bool> ExistAsync(Department department)
        {
            //checking if the department exist from the database return true/false;
            return await _context.Departments!
                .AsNoTracking()
                .AnyAsync(x => x.DepartmentName.ToLower().Equals(department.DepartmentName.ToLower())
                 && x.OfficeId.Equals(department.OfficeId)
                && x.Description.ToLower().Equals(department.Description.ToLower()));
        }

    }
}
