using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class RentService : IRentService
    {
        private readonly ApplicationDbContext _context;

        public RentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Rent>> List(int page, int pageSize, RentSearch search)
        {       
            return await _context.Rent.GetPagedAsync(page, 5);
        }

        public async Task<Rent> Get(int id)
        {
            return await _context.Rent.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Rent list)
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
            var todoList = await _context.Rent.FindAsync(id);
            if (todoList != null)
            {
                _context.Rent.Remove(todoList);
                await _context.SaveChangesAsync();
            }
        }
    }
}

