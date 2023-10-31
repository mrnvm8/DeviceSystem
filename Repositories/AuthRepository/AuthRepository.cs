namespace DeviceSystem.Repositories.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;
        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistAsync(Guid employeeId) =>
            await _context.Users!.AnyAsync(u => u.EmployeeId.Equals(employeeId));

        public async Task<User?> Login(Guid employeeId) =>
            await _context.Users!
            .Include(e => e.Employee)
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
