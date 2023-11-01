using DeviceSystem.Requests.Ticket;
using DeviceSystem.Services.DeviceService;
using DeviceSystem.Services.TicketService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IDeviceService _deviceService;

        public TicketsController(ITicketService ticketService,
        IDeviceService deviceService)
        {
            _ticketService = ticketService;
            _deviceService = deviceService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _ticketService.GetTickets();
            if (result.Data is null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        public IActionResult Create(Guid id)
        {
            var Devices = (_deviceService.GetDevices().Result).Data;
            ViewData["DeviceId"] = Devices!.Where(d => d.Id.Equals(id))
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DeviceName,
            });

            return View();
        }

        // POST: DeviceLoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceId,TicketTitle,TicketIssue")] CreateTicketRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _ticketService.AddTicket(request);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Tickets");
                    // return RedirectToAction("Index", "Devices",
                    //    new { id = result.Data!.DepartmentId, typeId = result.Data.DeviceTypeId });
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        
        public async Task<IActionResult> AcknowledgeTicket(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.GetTicketById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcknowledgeTicket(Guid id, [Bind("ArangedDate,TicketUpdate")] UpdateTicketRequest request)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.TicketAcknowledge(request,id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ArchiveTicket(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.GetTicketById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveTicket(Guid id, [Bind("TicketSolution")] UpdateTicketRequest request)
        {
            if(id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.TicketArchived(request, id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }


        //Tickets/delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.GetTicketById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _ticketService.DeleteTicket(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
