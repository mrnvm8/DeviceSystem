using DeviceSystem.Requests.Office;
using DeviceSystem.Services.OfficeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OfficesController : Controller
    {
        private readonly IOfficeService _officeService;
        public OfficesController(IOfficeService officeService)
        {

            _officeService = officeService;
        }

        // GET: Offices
        public async Task<IActionResult> Index()
        {
            var result = await _officeService.GetOfficesList();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }


        // GET: Offices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Offices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location")] CreateOfficeRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _officeService.CreateOffice(request);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: Offices/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _officeService.GetOfficeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: Offices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Location")] UpdateOfficeRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _officeService.UpdateOffice(id, request);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Offices/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _officeService.GetOfficeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: Offices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _officeService.RemoveOffice(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
