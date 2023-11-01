using DeviceSystem.Requests.Ticket;

namespace DeviceSystem.Mapping
{
    public static class TicketMapping
    {
        public static Ticket CreateMapToTicket(this CreateTicketRequest request, Guid userId)
        {
            return new Ticket
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeviceId = request.DeviceId,
                TicketTitle = request.TicketTitle,
                TicketIssue = request.TicketIssue,
                TicketCreatedDate = DateTime.Now
            };
        }

        public static Ticket UpdateMapToTicket(this UpdateTicketRequest request, Guid ticketId)
        {
            return new Ticket
            {
                Id = ticketId,
                ArangedDate = request.ArangedDate,
                TicketUpdate = request.TicketUpdate,
                FixedDate = request.FixedDate,
                TicketSolution = request.TicketSolution,
            };
        }

        public static TicketResponse MapTicketResponse(this Ticket ticket)
        {
            return new TicketResponse
            {
                Id = ticket.Id,
                TicketTitle = ticket.TicketTitle,
                TicketIssue = ticket.TicketIssue,
                TicketSolution = ticket.TicketSolution,
                TicketCreatedDate = ticket.TicketCreatedDate,
                ArangedDate = ticket.ArangedDate,
                FixedDate = ticket.FixedDate,
                TicketUpdate = ticket.TicketUpdate,
                IssueSolved = ticket.IssueSolved,
                Updated = ticket.Updated,
                DeviceName = ticket.Devices!.DeviceName,
                DeviceType = ticket.Devices!.DeviceType?.Name,
                Department = ticket.Devices!.Department?.DepartmentName,
            };
        }
    }
}
