﻿using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ICarsService
    {
        Task<PagedResult<Cars>> List(int page, int pageSize);
        Task<Cars> Get(int id);
        Task Save(Cars list);
        Task Delete(int id);
    }
}
