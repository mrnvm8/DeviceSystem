
namespace DeviceSystem.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;
        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> ChangePassword(User user)
        {
            //attach user entity
            _context.Attach(user);
            //updating user to the database
            _context.Entry(user).State = EntityState.Modified;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<int> DeleteUser(User user)
        {
            //attach user entity
            _context.Attach(user);
            //Delete user to the database
            _context.Entry(user).State = EntityState.Deleted;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

        public async Task<bool> ExistAsync(Guid employeeId) =>
            await _context.Users!.AnyAsync(u => u.EmployeeId.Equals(employeeId));

        public async Task<List<User>> GetUsers() =>
            await _context.Users!
            .Include(e =>e.Employee)
                .ThenInclude(p =>p!.Person)
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Department)
                    .ThenInclude(o => o!.Offices)
            .AsNoTracking()
            .ToListAsync();

        public async Task<User?> GetUserById(Guid userId) =>
             await _context.Users!
             .AsNoTracking()
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Person)
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Department)
                    .ThenInclude(o => o!.Offices)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId));

        public async Task<User?> Login(Guid employeeId) =>
            await _context.Users!
             .AsNoTracking()
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Person)
            .Include(e => e.Employee)
                .ThenInclude(p => p!.Department)
                    .ThenInclude(o => o!.Offices)
            .FirstOrDefaultAsync(x => x.EmployeeId.Equals(employeeId));

        public async Task<int> Register(User user)
        {
            //attach user entity
            _context.Attach(user);
            //Adding user to the database
            _context.Entry(user).State = EntityState.Added;
            //getting the status if a row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            return affected;
        }

    }
}
