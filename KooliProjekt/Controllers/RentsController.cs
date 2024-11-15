using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    public class RentController : Controller
    {
        private readonly IRentsService _rentService;

        public RentController(IRentsService RentService)
        {
            _rentService = RentService;
        }

        // GET: TodoLists
        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _rentService.List(page, 5);

            return View(data);
        }

        // GET: TodoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Rent = await _rentService.Get(id.Value);
            if (Rent == null)
            {
                return NotFound();
            }

            return View(Rent);
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
        public async Task<IActionResult> Create([Bind("Id,Title")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                await _rentService.Save(rent);
                return RedirectToAction(nameof(Index));
            }
            return View(rent);
        }

        // GET: TodoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _rentService.Get(id.Value);
            if (rent == null)
            {
                return NotFound();
            }
            return View(rent);
        }

        // POST: TodoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _rentService.Save(rent);
                return RedirectToAction(nameof(Index));
            }
            return View(rent);
        }

        // GET: TodoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _rentService.Get(id.Value);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: TodoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _rentService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}