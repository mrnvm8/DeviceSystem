namespace DeviceSystem.Repositories.TicketRepository
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetTickets();
        Task<Ticket?> GetTicketById(Guid ticketId);
        Task<int> UpdateTicket(Ticket ticket);
        Task<int> AddTicket(Ticket ticket);
        Task<int> DeleteTicket(Ticket ticket);
        Task<bool> ExistTicketAsync(Ticket ticket);
    }
}
