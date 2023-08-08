using OrderService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task SaveActiveProductAsync(ActiveProduct product);

        Task RemoveActiveProductAsync(ActiveProduct product);

        Task<ActiveProduct> GetActiveProductAsync(Guid identifier);

        Task<List<ActiveProduct>> GetActiveProductsAsync(List<Guid> products);
    }
}
