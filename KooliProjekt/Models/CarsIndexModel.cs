using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class CarsIndexModel
    {
        public CarsSearch Search { get; set; } = new CarsSearch();
        public PagedResult<Cars> Data { get; set; } = new PagedResult<Cars>();
    }
}

