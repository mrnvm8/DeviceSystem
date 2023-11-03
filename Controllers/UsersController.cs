using System.Security.Claims;
using DeviceSystem.Services.AuthService;
using DeviceSystem.Services.EmployeeService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;

        public UsersController(IAuthService authService, IEmployeeService employeeService)
        {
            _authService = authService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authService.GetUsersFromDB();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }


        // GET: User Login Page
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Post: Verifying Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(request.Email, request.Password);

                if (result.Success)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.Data!.UserId.ToString()),
                        new Claim(ClaimTypes.Email, result.Data!.Email.ToString()),
                        new Claim(ClaimTypes.Role, result.Data!.Role.ToString()),
                        new Claim(ClaimTypes.Name, result.Data!.EmployeeId.ToString()),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = false,
                        IssuedUtc = DateTime.UtcNow,
                        RedirectUri = "~/Users/Login"
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: Users/Create
        public IActionResult Register()
        {
            //get the list of employees
            var Employees = (_employeeService.GetEmployees().Result).Data;
            ViewData["EmployeeId"] = Employees!.DistinctBy(x => x.PersonId).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.WorkEmail
            });
            return View();
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("EmployeeId,Password,ConfirmPassword,Role")] CreateUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(
                    new CreateUserRequest
                    {
                        EmployeeId = request.EmployeeId,
                        Role = request.Role
                    }, request.Password);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                //convering the string to array of error message
                ViewBag.Error = result.Message!.Split(",").Reverse();
            }
            else
            {
                ViewBag.Error = "Invalid Information";
            }
            return View();
        }

        public IActionResult PasswordChange(Guid id)
        {
            return View();
        }

        // POST: Users/passwordChange
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(Guid id, [Bind("Password,ConfirmPassword")] ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.ChangeUserPassword(id, request.Password);
                if (result.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                //convering the string to array of error message
                ViewBag.Error = result.Message!.Split(",").Reverse();
            }
            else
            {
                ViewBag.Error = "Invalid Information";
            }
            return View();
        }

        public async Task<IActionResult> Delete(Guid id)
        {

            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _authService.GetUserById(id);
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

            var result = await _authService.RemoveUser(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}