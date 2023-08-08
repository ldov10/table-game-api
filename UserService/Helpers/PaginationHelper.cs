using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Models.Pagination;

namespace UserService.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<List<TItem>> GetPagedListAsync<TItem>(IQueryable<TItem> query,
            PaginationParameters paginationParameters)
        {
            var items = await query
                .Skip((paginationParameters.PageIndex - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new List<TItem>(items);
        }
    }
}
