using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CarsService : ICarsService
    {
        private readonly ApplicationDbContext _context;

        public CarsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Cars>> List(int page, int pageSize, CarsSearch search)
        {
            IQueryable<Cars> query = _context.Cars;

            // Apply search filters
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.Keyword))
                {
                    query = query.Where(car => car.Title.Contains(search.Keyword));
                }

                if (search.Done.HasValue)
                {
                    query = query.Where(car => car.is_available == search.Done.Value);
                }
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Cars> Get(int id)
        {
            return await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Cars list)
        {
            if (list.Id == 0)
            {
                _context.Add(list);
            }
            else
            {
                _context.Update(list);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car != null)
            {
                _context.Cars.Remove(Car);
                await _context.SaveChangesAsync();
            }
        }
    }
}