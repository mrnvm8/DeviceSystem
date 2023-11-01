using System;

namespace DeviceSystem.Repositories.TicketRepository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> AddTicket(Ticket ticket)
        {
            //Adding Ticket to the database
            _context.Tickets!.Add(ticket);
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<int> DeleteTicket(Ticket ticket)
        {
            //Deleting Ticket to the database
            _context.Tickets!.Remove(ticket);
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }

        public async Task<bool> ExistTicketAsync(Ticket ticket) =>
            await _context.Tickets!
            .AsNoTracking()
            .AnyAsync(x => x.DeviceId.Equals(ticket.DeviceId) 
            && x.IssueSolved.Equals(false));
            
        public async Task<Ticket?> GetTicketById(Guid ticketId)=>
            await _context.Tickets!
            .Include(d => d.Devices)
                .ThenInclude(x => x!.Department)
            .Include(d => d.Devices)
                .ThenInclude(d => d!.DeviceType)
            .Include(u => u.Users)
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>x.Id.Equals(ticketId));
            
        public async Task<List<Ticket>> GetTickets() => 
            await _context.Tickets!
            .AsNoTracking()
            .Include(d => d.Devices)
                .ThenInclude(x => x!.Department)
            .Include(d => d.Devices)
                .ThenInclude(d => d!.DeviceType)
            .Include(u=>u.Users)
            .ToListAsync();

        public async Task<int> UpdateTicket(Ticket ticket)
        {
            //updating the ticket
            //_context.Tickets!.Update(ticket);
            _context.Attach(ticket);
            _context.Entry(ticket).State = EntityState.Modified;
            //getting the status if a row was updated from the database 
            var affected = await _context.SaveChangesAsync();
            //return the 1
            return affected;
        }
    }
}
