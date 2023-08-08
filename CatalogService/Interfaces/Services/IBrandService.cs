using CatalogService.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.Interfaces.Services
{
    public interface IBrandService
    {
        Task PostBrandAsync(BrandCreationDto brand);

        Task UpdateBrandAsync(Guid identifier, BrandUpdateDto brand);

        Task<List<BrandDetails>> GetBrandListAsync();

        Task RemoveBrandAsync(Guid identifier);
    }
}
