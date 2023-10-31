using DeviceSystem.Requests.DeviceLoans;
using DeviceSystem.Services.DeviceLoanService;
using DeviceSystem.Services.DeviceService;
using DeviceSystem.Services.EmployeeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace DeviceSystem.Controllers
{
    [Authorize]
    public class DeviceLoansController : Controller
    {

        private readonly IDeviceLoanService _deviceLoan;
        private readonly IDeviceService _deviceService;
        private readonly IEmployeeService _employeeService;

        public DeviceLoansController(IDeviceLoanService deviceLoan,
        IDeviceService deviceService,
        IEmployeeService employeeService)
        {

            _deviceLoan = deviceLoan;
            _deviceService = deviceService;
            _employeeService = employeeService;
        }

        // GET: DeviceLoans/Details
        [ActionName("Details")]
        public async Task<IActionResult> DeviceDetailsAndHistory(Guid id)
        {
           
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }
            
            var devices = await _deviceService.GetDeviceById(id);
            ViewData["Device"] = devices.Data;

            var result = await _deviceLoan.GetAllDeviceLoansById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
           
        }

        // GET: DeviceLoans/Assign
        public IActionResult Assign(Guid id)
        {
            //getting the list of employees and devices
            var Employees = (_employeeService.GetEmployees().Result).Data;
            var Devices = (_deviceService.GetDevices().Result).Data;
            ViewData["DeviceId"] = Devices!.Where(d => d.Id.Equals(id)).Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DeviceName,
            });
            ViewData["EmployeeId"] = Employees!.DistinctBy(x => x.PersonId).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.FullName
            });
            return View();
        }

        // POST: DeviceLoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign([Bind("DeviceId,EmployeeId")] CreateDeviceLoanRequest request)
        {
            if (ModelState.IsValid)
            {
                request.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result = await _deviceLoan.AssignDevice(request);
                if (result.Success)
                {
                    return RedirectToAction("DeviceSummary", "Devices");
                    // return RedirectToAction("Index", "Devices",
                    //    new { id = result.Data!.DepartmentId, typeId = result.Data.DeviceTypeId });
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: DeviceLoans/Unassign/5
        public async Task<IActionResult> Unassign(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _deviceLoan.GetDeviceLoanById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            var Employees = (_employeeService.GetEmployees().Result).Data;
            var Devices = (_deviceService.GetDevices().Result).Data;
            ViewData["DeviceId"] = Devices!.Where(d => d.Id.Equals(id)).Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DeviceName,
            });
            ViewData["EmployeeId"] = Employees!.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.FullName
            });
            return View(result.Data);
        }

        // POST: DeviceLoans/Unassign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unassign(Guid id, [Bind("DeviceId,EmployeeId")] UpdateDeviceLoanRequest request)
        {
            request.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (ModelState.IsValid)
            {
                var result = await _deviceLoan.UnassignedDevice(id, request);
                if (result.Success)
                {
                    return RedirectToAction("DeviceSummary", "Devices");
                    // return RedirectToAction("Index", "Devices",
                    //      new { id = result.Data!.DepartmentId, typeId = result.Data.DeviceTypeId });
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }
    }
}
