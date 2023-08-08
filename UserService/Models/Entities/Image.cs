using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public class Image : BaseEntity
    {
        [Required]
        public byte[] Data { get; set; }

        public long UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}
