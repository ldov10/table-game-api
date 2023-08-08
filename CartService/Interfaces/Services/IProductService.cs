using System.Threading.Tasks;
using System;
using CartService.Models.Entities;

namespace CartService.Interfaces.Services
{
    public interface IProductService
    {
        Task AddActiveProductAsync(Guid productIdentifier);

        Task RemoveActiveProductAsync(Guid productIdentifier);
    }
}
