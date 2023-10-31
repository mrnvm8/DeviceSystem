namespace DeviceSystem.Repositories.EmployeeRespository
{
    public class EmployeeRespository : IEmployeeRespository
    {
        private readonly ApplicationContext _context;
        public EmployeeRespository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployeesAsync() => 
            await _context.Employees!
            .Include(d => d.Department)
                .ThenInclude(o => o!.Offices)
            .Include(p => p.Person)
            .AsNoTracking()
            .ToListAsync();

        public async Task<Employee?> GetEmployeeByIdAsync(Guid employeeId) => 
            await _context.Employees!
            .Include(d => d.Department)
                .ThenInclude(o => o!.Offices)
            .Include(p => p.Person)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == employeeId);

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            //Adding employee to the database
            _context.Employees!.Add(employee);
            //getting the status if  any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> DeleteEmployeeAsync(Employee employee)
        {
            //remove the employee
            //_context.Employees!.Remove(employee);
            _context.Attach(employee);
            _context.Entry(employee).State = EntityState.Deleted;
            //getting the status if any row was affected from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            
            //updating the employee
            _context.Attach(employee);
            _context.Entry(employee).State = EntityState.Modified;
            //_context.Update<Employee>(employee);
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }
        public async Task<bool> ExistEmployeeAsync(Employee employee)
        {
            //checking if the employee exist with same department
            //from the database return true/false;
            return await _context.Employees!
                     .AsNoTracking()
                     .AnyAsync(x => x.PersonId.Equals(employee.PersonId)
                     && x.DepartmentId.Equals(employee.DepartmentId)
                     && x.WorkEmail.ToLower().Equals(employee.WorkEmail.ToLower()));
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email) => 
             await _context.Employees!.
                 FirstOrDefaultAsync(x => x.WorkEmail.ToLower()
                .Equals(email.ToLower()));
    }
}
