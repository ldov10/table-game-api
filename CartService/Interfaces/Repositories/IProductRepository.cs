using CartService.Models.Entities;
using System;
using System.Threading.Tasks;

namespace CartService.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task SaveActiveProductAsync(ActiveProduct product);

        Task RemoveActiveProductAsync(ActiveProduct product);

        Task<ActiveProduct> GetActiveProductAsync(Guid identifier);
    }
}
