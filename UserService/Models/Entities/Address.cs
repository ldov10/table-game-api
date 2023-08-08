using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public class Address : BaseEntity
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string AddressString { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        public string Zip { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Country { get; set; }

        public long UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
