using DeviceSystem.Requests.Employee;
using DeviceSystem.Services.DepartmentService;
using DeviceSystem.Services.EmployeeService;
using DeviceSystem.Services.PersonService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IPeopleService _personService;

        public EmployeesController(IEmployeeService employeeService,
        IDepartmentService departmentService,
        IPeopleService personService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _personService = personService;
        }



        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var result = await _employeeService.GetEmployees();
            if (result.Data is null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        [ActionName("Details")]
        public async Task<IActionResult> DeviceAssignedToEmployee(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _employeeService.GetDeviceLoanById(id);
            if (result.Data is null)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            return View(result.Data);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {

            //getting the list of department and people
            var Departments = (_departmentService.GetDepartmentsList().Result).Data;
            var People = (_personService.GetPeopleList().Result).Data;

            ViewData["DepartmentId"] = Departments!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.OfficeName} => {d.DepartmentName}"
            });

            ViewData["PersonId"] = People!.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.FirstName}, {p.LastName}"
            });

            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,DepartmentId,WorkEmail")] CreateEmployeeRequest employee)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.CreateEmployee(employee);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _employeeService.GetEmployeeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            var Departments = (_departmentService.GetDepartmentsList().Result).Data;

            ViewData["DepartmentId"] = Departments!.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.OfficeName} => {d.DepartmentName}"
            });

            return View(result.Data);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DepartmentId,WorkEmail")] UpdateEmployeeRequest employee)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }
            if (ModelState.IsValid)
            {
                var result = await _employeeService.UpdateEmployee(id, employee);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _employeeService.GetEmployeeById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _employeeService.RemoveEmployee(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
