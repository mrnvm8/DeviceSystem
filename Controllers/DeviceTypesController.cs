using DeviceSystem.Requests.DeviceType;
using DeviceSystem.Services.DeviceTypeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeviceTypesController : Controller
    {
        private readonly IDeviceTypeService _typeService;

        public DeviceTypesController(IDeviceTypeService typeService)
        {
            _typeService = typeService;
        }

        // GET: DeviceTypes
        public async Task<IActionResult> Index()
        {
            var result = await _typeService.GetDeviceTypesList();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        // GET: DeviceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeviceTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] CreateTypeRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _typeService.AddDeviceType(request);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: DeviceTypes/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _typeService.GetDeviceTypeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: DeviceTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] UpdateTypeRequest request)
        {

            if (ModelState.IsValid)
            {
                var result = await _typeService.UpdateDeviceType(id, request);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: DeviceTypes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _typeService.GetDeviceTypeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: DeviceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _typeService.RemoveDeviceType(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
