using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Models.Entities;

namespace CatalogService.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task SaveImageAsync(Image image);

        Task RemoveProductImagesAsync(long productId);

        Task<Image> GetProductImageAsync(Guid identifier);

        Task<Image> GetProductFirstImageAsync(long productId);

        Task<List<Guid>> GetProductImagesIdsAsync(long productId);
    }
}
