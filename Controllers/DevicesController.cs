using DeviceSystem.Requests.Device;
using DeviceSystem.Services.DepartmentService;
using DeviceSystem.Services.DeviceService;
using DeviceSystem.Services.DeviceTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceSystem.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDeviceTypeService _deviceTypeService;
        private readonly IDeviceService _deviceService;
      
        public DevicesController(ApplicationContext context,
        IDepartmentService departmentService,
        IDeviceTypeService deviceTypeService, 
        IDeviceService deviceService)
        {
            _departmentService = departmentService;
            _deviceTypeService = deviceTypeService;
            _deviceService = deviceService;
        }

        // GET: Devices/departId?devicetypeId
        public async Task<IActionResult> AllDevices()
        {
            var result = await _deviceService.GetDevices();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        // GET: Devices/departId?devicetypeId
        public async Task<IActionResult> Index(Guid Id, Guid typeId)
        {
          
            if (Id == Guid.Empty || typeId == Guid.Empty)
            {
                ViewBag.Error = "Invalid path(s)";
                return View();
            }
            var result = await _deviceService.GetDevicesList(Id, typeId);
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        //GET Device/DeviceSummary
        public async Task<IActionResult> DeviceSummary()
        {
            var result = await _deviceService.Summary();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        // GET: Devices/Create
        public IActionResult Create()
        {
            var Departments = (_departmentService.GetDepartmentsList().Result).Data;
            var DeviceTypes = (_deviceTypeService.GetDeviceTypesList().Result).Data;

            ViewData["DepartmentId"] = Departments!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.OfficeName} => {d.DepartmentName}"
            });

            ViewData["DeviceTypeId"] = DeviceTypes!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Name}"
            });
            return View();
        }

        // POST: Devices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartId,TypeId,DeviceName,SerialNo," +
            "IMEINo,Condition,PurchasedPrice,PurchasedDate")] CreateDeviceRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _deviceService.AddDevice(request);
                if (result.Success)
                {
                    return RedirectToAction("Index","Devices", 
                        new {id = result.Data!.DepartId, typeId = result.Data.TypeId});
                }
                ViewBag.Error = result.Message;
            }
            
            return View();
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _deviceService.GetDeviceById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }


            var Departments = (_departmentService.GetDepartmentsList().Result).Data;
            var DeviceTypes = (_deviceTypeService.GetDeviceTypesList().Result).Data;

            ViewData["DepartmentId"] = Departments!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.OfficeName} => {d.DepartmentName}"
            });

            ViewData["DeviceTypeId"] = DeviceTypes!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Name}"
            });

            return View(result.Data);
        }

        // POST: Devices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DepartId,TypeId,DeviceName,SerialNo," +
            "IMEINo,Condition,PurchasedPrice,PurchasedDate,Year")] UpdateDeviceRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _deviceService.UpdateDevice(id, request);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Devices",
                        new { id = result.Data!.DepartId, typeId = result.Data.TypeId });
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _deviceService.GetDeviceById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _deviceService.RemoveDevice(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
