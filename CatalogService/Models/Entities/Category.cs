using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models.Entities
{
    public class Category : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Title { get; set; }
    }
}
