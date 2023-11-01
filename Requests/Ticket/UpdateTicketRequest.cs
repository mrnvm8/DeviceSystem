using System;

namespace DeviceSystem.Requests.Ticket
{
    public class UpdateTicketRequest
    {
        public Guid DeviceId { get; set; }
        public DateTime? ArangedDate { get; init; }
        public DateTime? FixedDate { get; init; }
        public string? TicketSolution { get; init; }
        public string? TicketUpdate { get; init; }
    }
}
