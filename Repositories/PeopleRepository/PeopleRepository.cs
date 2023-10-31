namespace DeviceSystem.Repositories.PeopleRepository
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationContext _context;

        public PeopleRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetPeopleAsync() =>
            await _context.People!
            .AsNoTracking()
            .ToListAsync();

        public async Task<Person?> GetPersonByIdAsync(Guid personId) =>
            await _context.People!
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == personId);

        public async Task<int> CreatePersonAsync(Person person)
        {
            //Adding person to the database
            _context.People!.Add(person);
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> DeletePersonAsync(Person person)
        {
            //remove the person
            _context.People!.Remove(person);
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> UpdatePersonAsync(Person person)
        {
            //updating the person
            //_context.People!.Update(person);
            _context.Attach(person);
            _context.Entry(person).State = EntityState.Modified;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<bool> ExistPersonAsync(Person person)
        {
            //checking if the person exist from the database return true/false;
            return await _context.People!
                     .AsNoTracking()
                     .AnyAsync(x => x.FirstName.ToLower()
                     .Equals(person.FirstName.ToLower())
                     && x.LastName.ToLower()
                     .Equals(person.LastName.ToLower())
                     && x.Gender.Equals(person.Gender));
        }


    }
}
