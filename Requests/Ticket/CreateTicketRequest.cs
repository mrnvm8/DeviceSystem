using System;

namespace DeviceSystem.Requests.Ticket
{
    public class CreateTicketRequest
    {
        public Guid DeviceId {get; set;}
        public string TicketTitle { get; set; } = string.Empty;
        public string TicketIssue { get; set; } = string.Empty;
        public DateTime TicketCreatedDate { get; set; }
    }
}
