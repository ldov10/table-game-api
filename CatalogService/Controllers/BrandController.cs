using System;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("postBrand")]
        public async Task<IActionResult> PostBrand([FromBody] BrandCreationDto brand)
        {
            await _brandService.PostBrandAsync(brand);
            return Ok();
        }

        [HttpPut("updateBrand/{identifier}")]
        public async Task<IActionResult> UpdateBrand(Guid identifier, [FromBody] BrandUpdateDto brand)
        {
            await _brandService.UpdateBrandAsync(identifier, brand);
            return Ok();
        }

        [HttpGet("getBrandList")]
        public async Task<IActionResult> GetBrandList()
        {
            return Ok(await _brandService.GetBrandListAsync());
        }

        [HttpDelete("deleteBrand/{identifier}")]
        public async Task<IActionResult> DeleteBrand(Guid identifier)
        {
            await _brandService.RemoveBrandAsync(identifier);
            return Ok();
        }
    }
}
