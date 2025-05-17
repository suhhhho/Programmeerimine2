using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
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

        // GET: Cars
        public async Task<IActionResult> Index(int page, CarsSearch search)
        {
            var data = await _carsService.List(page, 5, search);
            var model = new CarsIndexModel { Search = search, Data = data };
            return View(model);
        }

        // GET: Cars/Details/5
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

        // GET: Cars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,rental_rate_per_minute,rental_rate_per_km,is_available")] Cars cars)
        {
            if (ModelState.IsValid)
            {
                await _carsService.Save(cars);
                return RedirectToAction(nameof(Index));
            }
            return View(cars);
        }

        // GET: Cars/Edit/5
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

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,rental_rate_per_minute,rental_rate_per_km,is_available")] Cars cars)
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

        // GET: Cars/Delete/5
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

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}