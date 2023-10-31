using DeviceSystem.Requests.Person;
using DeviceSystem.Services.PersonService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PeopleController : Controller
    {
        private readonly IPeopleService _personService;
        public PeopleController(IPeopleService personService)
        {
            _personService = personService;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var result = await _personService.GetPeopleList();
            if (result.Data == null)
            {
                ViewBag.Error = result.Message;
            }
            return View(result.Data);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Gender")] CreatePersonRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _personService.CreatePerson(request);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Error = result.Message;
            }
            return View();
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _personService.GetPersonById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,LastName,Gender")] UpdatePersonRequest person)
        {
            if (ModelState.IsValid)
            {
                var result = await _personService.UpdatePerson(id, person);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _personService.GetPersonById(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                ViewBag.Error = "Invalid Id";
                return View();
            }

            var result = await _personService.RemovePerson(id);
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
