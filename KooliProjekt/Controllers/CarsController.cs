using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService _carsService;

        public CarsController(ICarsService CarsService)
        {
            _carsService = CarsService;
        }

        // GET: TodoLists
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _carsService.List(page, 5);

            return View(data);
        }

        // GET: TodoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _carsService.Get(id.Value);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        // GET: TodoLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Cars cars)
        {
            if (ModelState.IsValid)
            {
                await _carsService.Save(cars);
                return RedirectToAction(nameof(Index));
            }
            return View(cars);
        }

        // GET: TodoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _carsService.Get(id.Value);
            if (cars == null)
            {
                return NotFound();
            }
            return View(cars);
        }

        // POST: TodoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Cars cars)
        {
            if (id != cars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _carsService.Save(cars);
                return RedirectToAction(nameof(Index));
            }
            return View(cars);
        }

        // GET: TodoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _carsService.Get(id.Value);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        // POST: TodoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carsService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}