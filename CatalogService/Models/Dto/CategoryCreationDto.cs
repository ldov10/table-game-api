using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Dto
{
    public class CategoryCreationDto
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Title { get; set; }
    }
}
