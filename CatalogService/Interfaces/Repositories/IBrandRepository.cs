using CatalogService.Models.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CatalogService.Interfaces.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandAsync(Guid identifier);

        Task<Brand> GetBrandAsync(string title);

        Task SaveBrandAsync(Brand brand);

        Task UpdateBrandAsync(Brand brand);

        Task<List<Brand>> GetBrandListAsync();

        Task RemoveBrandAsync(Brand brand);
    }
}
