using System;
using CatalogService.Interfaces.Services;
using CatalogService.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("postCategory")]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreationDto category)
        {
            await _categoryService.PostCategoryAsync(category);
            return Ok();
        }

        [HttpPut("updateCategory/{identifier}")]
        public async Task<IActionResult> UpdateCategory(Guid identifier, [FromBody] CategoryUpdateDto category)
        {
            await _categoryService.UpdateCategoryAsync(identifier, category);
            return Ok();
        }

        [HttpGet("getCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            return Ok(await _categoryService.GetCategoryListAsync());
        }

        [HttpDelete("deleteCategory/{identifier}")]
        public async Task<IActionResult> DeleteCategory(Guid identifier)
        {
            await _categoryService.RemoveCategoryAsync(identifier);
            return Ok();
        }
    }
}
