using System;

namespace UserService.Models.Dto
{
    public class ProductReview
    {
        public Guid UserIdentifier { get; set; }

        public string UserFirstName { get; set; }

        public int Rating { get; set; }

        public string Description { get; set; }
    }
}
