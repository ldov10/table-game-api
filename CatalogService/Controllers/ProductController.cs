using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using CatalogService.Models.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("postProduct")]
        public async Task<IActionResult> PostProduct([FromBody] ProductCreationDto product)
        {
            await _productService.PostProductAsync(product);
            return Ok();
        }

        [HttpGet("getProductPage/pageIndex/{pageIndex}/pageSize/{pageSize}")]
        public async Task<IActionResult> GetProductPage(int pageIndex, int pageSize, [FromQuery] ProductsSearchOptions productsSearchOptions)
        {
            var pagination = new PaginationParameters(pageIndex, pageSize);

            return Ok(await _productService.GetProductPageAsync(pagination, productsSearchOptions));
        }

        [HttpGet("getInactiveProductPage/pageIndex/{pageIndex}/pageSize/{pageSize}")]
        public async Task<IActionResult> GetInactiveProductPage(int pageIndex, int pageSize, [FromQuery] ProductsSearchOptions productsSearchOptions)
        {
            var pagination = new PaginationParameters(pageIndex, pageSize);

            return Ok(await _productService.GetProductPageAsync(pagination, productsSearchOptions, false));
        }

        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts([FromQuery] List<Guid> productIds)
        {
            return Ok(await _productService.GetProductsAsync(productIds));
        }

        [HttpGet("getProductDetails/{identifier}")]
        public async Task<IActionResult> GetProductDetails(Guid identifier)
        {
            return Ok(await _productService.GetProductDetailsAsync(identifier));
        }

        [HttpPut("updateProduct/{identifier}")]
        public async Task<IActionResult> UpdateProduct(Guid identifier, [FromBody] ProductUpdateDto product)
        {
            await _productService.UpdateProductAsync(identifier, product);
            return Ok();
        }

        [HttpPut("updateProductPrice/{identifier}")]
        public async Task<IActionResult> UpdateProductPrice(Guid identifier, [FromBody] decimal price)
        {
            await _productService.UpdateProductPriceAsync(identifier, price);
            return Ok();
        }

        [HttpPut("updateProductActiveState/{identifier}")]
        public async Task<IActionResult> UpdateProductActiveState(Guid identifier, [FromBody] bool isActive)
        {
            await _productService.UpdateProductActiveStateAsync(identifier, isActive);
            return Ok();
        }

        [HttpDelete("deleteProduct/{identifier}")]
        public async Task<IActionResult> DeleteProduct(Guid identifier)
        {
            await _productService.DeleteProductAsync(identifier);
            return Ok();
        }

        [HttpPost("postProductImage/{identifier}")]
        public async Task<IActionResult> PostProductImage(Guid identifier, IFormFile file)
        {
            await _productService.PostProductImageAsync(identifier, file);
            return Ok();
        }

        [HttpGet("getProductImage/{identifier}")]
        public async Task<IActionResult> GetProductImage(Guid identifier)
        {
            var image = await _productService.GetProductImageAsync(identifier);
            return File(image, "image/jpeg");
        }

        [HttpGet("getProductImage/product/{identifier}")]
        public async Task<IActionResult> GetProductFirstImage(Guid identifier)
        {
            var image = await _productService.GetProductFirstImageAsync(identifier);
            return File(image, "image/jpeg");
        }

        [HttpGet("getProductImagesIds/{identifier}")]
        public async Task<IActionResult> GetProductImagesIds(Guid identifier)
        {
            return Ok(await _productService.GetProductImagesIdsAsync(identifier));
        }

        [HttpPost("postBookmark/user/{userIdentifier}/product/{productIdentifier}")]
        public async Task<IActionResult> PostBookmark(Guid userIdentifier, Guid productIdentifier)
        {
            await _productService.PostBookmarkAsync(userIdentifier, productIdentifier);
            return Ok();
        }

        [HttpPost("deleteBookmark/user/{userIdentifier}/bookmark/{bookmarkIdentifier}")]
        public async Task<IActionResult> DeleteBookmark(Guid userIdentifier, Guid bookmarkIdentifier)
        {
            await _productService.DeleteBookmarkAsync(userIdentifier, bookmarkIdentifier);
            return Ok();
        }

        [HttpPost("getUserBookmarks/{identifier}")]
        public async Task<IActionResult> GetUserBookmarks(Guid identifier)
        {
            return Ok(await _productService.GetUserBookmarksAsync(identifier));
        }
    }
}
