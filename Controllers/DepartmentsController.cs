using DeviceSystem.Requests.Department;
using DeviceSystem.Services.DepartmentService;
using DeviceSystem.Services.OfficeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IOfficeService _officeService;
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IOfficeService officeService,
        IDepartmentService departmentService)
        {
            _officeService = officeService;
            _departmentService = departmentService;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var result = await _departmentService.GetDepartmentsList();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }


        // GET: Departments/Create
        public IActionResult Create()
        {
            //getting the list of offices
            var offices = (_officeService.GetOfficesList().Result).Data;
            //loadding the on a value to be passed to the cshtml(razor) page 
            ViewData["OfficeId"] = offices!.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name}"
            });
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfficeId,DepartmentName,Description")] CreateDepartmentRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _departmentService.CreateDepartment(request);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _departmentService.GetDepartmentById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            var offices = (_officeService.GetOfficesList().Result).Data;
            ViewData["OfficeId"] = offices!.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name}"
            });

            return View(result.Data);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OfficeId,DepartmentName,Description")] UpdateDepartmentRequest request)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }
            if (ModelState.IsValid)
            {
                var result = await _departmentService.UpdateDepartment(id, request);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _departmentService.GetDepartmentById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _departmentService.RemoveDepartment(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
