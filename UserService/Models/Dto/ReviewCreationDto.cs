using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dto
{
    public class ReviewCreationDto
    {
        [Required]
        public Guid ProductIdentifier { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(300)]
        public string Description { get; set; }
    }
}
