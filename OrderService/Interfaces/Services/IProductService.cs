using System.Threading.Tasks;
using System;
using OrderService.Models.Messages;

namespace OrderService.Interfaces.Services
{
    public interface IProductService
    {
        Task AddActiveProductAsync(ProductActivatedMessage message);

        Task RemoveActiveProductAsync(Guid productIdentifier);
    }
}
