using KooliProjekt.Data;
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

        public async Task<PagedResult<Cars>> List(int page, int pageSize)
        {
            return await _context.Cars.GetPagedAsync(page, 5);
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
            var todoList = await _context.Cars.FindAsync(id);
            if (todoList != null)
            {
                _context.Cars.Remove(todoList);
                await _context.SaveChangesAsync();
            }
        }
    }

    public interface ICarService
    {
        Task Delete(int id);
        Task<string?> List(int page, int v);
        Task Save(Cars cars);
        Task<Cars> Get(int id);
    }
}
