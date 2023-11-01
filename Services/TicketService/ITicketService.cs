using DeviceSystem.Requests.Ticket;

namespace DeviceSystem.Services.TicketService
{
    public interface ITicketService
    {
        Task<ServiceResponse<List<TicketResponse>>> GetTickets();
        Task<ServiceResponse<TicketResponse>> GetTicketById(Guid ticketId);
        Task<ServiceResponse<bool>> AddTicket(CreateTicketRequest request);
        Task<ServiceResponse<bool>> DeleteTicket(Guid ticketId);
        Task<ServiceResponse<bool>> TicketArchived(UpdateTicketRequest request, Guid ticketId);
        Task<ServiceResponse<bool>> TicketAcknowledge(UpdateTicketRequest request, Guid ticketId);
    }
}
