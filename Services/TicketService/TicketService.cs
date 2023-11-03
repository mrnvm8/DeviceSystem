using DeviceSystem.Mapping;
using DeviceSystem.Repositories.TicketRepository;
using DeviceSystem.Requests.Ticket;
using DeviceSystem.Services.AuthService;
using DeviceSystem.Services.DeviceService;
using DeviceSystem.Services.EmployeeService;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;


namespace DeviceSystem.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;
        private readonly IDeviceService _deviceService;
        private readonly IConfiguration _config;

        public TicketService(ITicketRepository ticketRepository,
        IAuthService authService,
        IEmployeeService employeeService,
        IDeviceService deviceService, IConfiguration config)
        {
            _ticketRepository = ticketRepository;
            _authService = authService;
            _employeeService = employeeService;
            _deviceService = deviceService;
            _config = config;
        }

        public async Task<ServiceResponse<bool>> AddTicket(CreateTicketRequest request)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to ticket Entity
            var ticket = request.CreateMapToTicket(_authService.GetUserId());

            //check if the same ticket is already in the Database
            var exist = await _ticketRepository.ExistTicketAsync(ticket);
            if (exist)
            {
                //if the ticket is already in the database then return false and error message
                response.Success = false;
                response.Message = $"Ticket already exist in the Ticket list for the same device.";
                return response;
            }
            else
            {
                //if the ticket is not in the database then create a new one
                var affected = await _ticketRepository.AddTicket(ticket);
                if (affected == 1)
                {
                    //sending the email to the user
                    SendTicketEmail(await ComposeEmailDetails(ticket));
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create the ticket";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> DeleteTicket(Guid ticketId)
        {
            var response = new ServiceResponse<bool>();
            //Get the ticket from the database by id
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket is null)
            {
                //if the ticket is not in the database then return false and error message
                response.Success = false;
                response.Message = "ticket not found.";
                return response;
            }
            else
            {
                //if the ticket exist delete the ticket
                var affected = await _ticketRepository.DeleteTicket(ticket);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete the ticket";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<TicketResponse>> GetTicketById(Guid ticketId)
        {
            var response = new ServiceResponse<TicketResponse>();
            // Get the Ticket from the database by Id
            var ticket = await _ticketRepository.GetTicketById(ticketId);

            if (ticket is null)
            {
                //if the Ticket is not in the database then return false and error message
                response.Success = false;
                response.Message = "Ticket not found.";
                return response;
            }
            else
            {
                //if the pTicket is in the database then map it to DTO 
                response.Data = ticket.MapTicketResponse();
                return response;
            }
        }

        public async Task<ServiceResponse<List<TicketResponse>>> GetTickets()
        {
            var response = new ServiceResponse<List<TicketResponse>>();
            var _ticketList = new List<TicketResponse>();

            //Get the list of tikets
            var _tickets = new List<Ticket>();

            //Wanting to display ticket for login user only or all the ticket for admin
            if (_authService.GetUserRole().Contains("Admin"))
            {
                _tickets = await _ticketRepository.GetTickets();
            }
            else
            {
                var userId = _authService.GetUserId();
                var tickets = await _ticketRepository.GetTickets();
                _tickets = tickets.Where(x => x.UserId.Equals(userId)).ToList();
            }

            //checking if there is any tickets
            if (_tickets.Count() == 0)
            {
                //if there are no people in the database
                response.Success = false;
                response.Message = "There are no ticket in the database yet.";
                return response;
            }
            else
            {
                //mapping each person to person response(dto)
                _tickets.ForEach(ticket =>
                {
                    _ticketList.Add(ticket.MapTicketResponse());
                });
                response.Data = _ticketList;

                return response;
            }
        }

        public async Task<ServiceResponse<bool>> TicketAcknowledge(UpdateTicketRequest request, Guid ticketId)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to ticket Entity
            var ticket = request.UpdateMapToTicket(ticketId);

            //get the ticket from the database by its id
            var dbTicket = await _ticketRepository.GetTicketById(ticketId);
            if (dbTicket is null)
            {
                //if the ticket is not in the database then return false and error message
                response.Success = false;
                response.Message = "Ticket not found.";
                return response;
            }
            else
            {
                //TODO: updating value
                dbTicket = new Ticket
                {
                    Id = ticket.Id,
                    UserId = dbTicket.UserId,
                    DeviceId = dbTicket.DeviceId,
                    TicketTitle = dbTicket.TicketTitle,
                    TicketIssue = dbTicket.TicketIssue,
                    TicketSolution = dbTicket.TicketSolution,
                    TicketCreatedDate = dbTicket.TicketCreatedDate,
                    FixedDate = dbTicket.FixedDate,
                    IssueSolved = dbTicket.IssueSolved,
                    ArangedDate = ticket.ArangedDate,
                    TicketUpdate = ticket.TicketUpdate,
                    Updated = true,
                };

                //if the ticket exist delete the person
                var affected = await _ticketRepository.UpdateTicket(dbTicket);
                if (affected == 1)
                {
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the ticket";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<bool>> TicketArchived(UpdateTicketRequest request, Guid ticketId)
        {
            var response = new ServiceResponse<bool>();
            //mapping the request to ticket Entity
            var ticket = request.UpdateMapToTicket(ticketId);

            //get the ticket from the database by its id
            var dbTicket = await _ticketRepository.GetTicketById(ticketId);
            if (dbTicket is null)
            {
                //if the person is not in the database then return false and error message
                response.Success = false;
                response.Message = "Ticket not found.";
                return response;
            }
            else
            {
                //TODO: updating value
                dbTicket = new Ticket
                {
                    Id = ticket.Id,
                    UserId = dbTicket.UserId,
                    DeviceId = dbTicket.DeviceId,
                    TicketTitle = dbTicket.TicketTitle,
                    TicketIssue = dbTicket.TicketIssue,
                    Updated = dbTicket.Updated,
                    TicketCreatedDate = dbTicket.TicketCreatedDate,
                    ArangedDate = dbTicket.ArangedDate,
                    TicketUpdate = dbTicket.TicketUpdate,
                    FixedDate = DateTime.Now,
                    TicketSolution = ticket.TicketSolution,
                    IssueSolved = true,
                };

                //if the ticket exist delete the person
                var affected = await _ticketRepository.UpdateTicket(dbTicket);
                if (affected == 1)
                {
                    //sending email when finished
                    //get the employee who created the ticket
                    var ticketCreator = await _authService.GetUserById(dbTicket.UserId);
                    SendTicketEmail(await ArchiveEmailDetails(ticketCreator.Data!, dbTicket));
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to update the ticket";
                    return response;
                }
            }
        }

        private async Task<EmailResponse> ComposeEmailDetails(Ticket ticket)
        {
            var EmailContext = new EmailResponse();
            var employee = (await _employeeService.GetEmployeeById(_authService.GetEmployeeId())).Data;
            var device = (await _deviceService.GetDeviceById(ticket.DeviceId)).Data;

            if (employee is not null && device is not null)
            {
                EmailContext.Subject = ticket.TicketTitle!;
                EmailContext.Body = $"Good day Tech Team <br/>" +
                                    $"<br/>I have created a Ticket about work device :<br/>" +
                                    $"<b>Device Name:</b> {device.DeviceName}<br/>" +
                                    $"<b>Identity Number:</b> {device.IdentityNumber}<br/>" +
                                    $"<h4>Here is the Issue:</h4>{ticket.TicketIssue}" +
                                    $"<br/><br/>Best regards<br/> {employee!.FullName}";

                EmailContext.Email = "techservices@axiumeducation.org";
            }

            return EmailContext;
        }

        private async Task<EmailResponse> ArchiveEmailDetails(UserResponse user, Ticket ticket)
        {
            var EmailContext = new EmailResponse();
            var employee = (await _employeeService.GetEmployeeById(_authService.GetEmployeeId())).Data;
            var device = (await _deviceService.GetDeviceById(ticket.DeviceId)).Data;

            EmailContext.Subject = ticket.TicketTitle!;
            EmailContext.Body = $"<h4> Good Day {user.FullName}</h4>" +
                                $"<p>I have Looked at the Ticket Title: {ticket.TicketTitle}</p>" +
                                $"<br><h4>Here is the Ticket Solution(s):<h/4><br>" +
                                $"<p>{ticket.TicketSolution}<p>" +
                                $"<br><br> Best regards <br> {employee!.FullName}";

            EmailContext.Email = user.Email;

            return EmailContext;
        }

        public void SendTicketEmail(EmailResponse request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(this._config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.Email));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

