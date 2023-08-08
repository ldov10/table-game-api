using System;

namespace CatalogService.Models.Dto
{
    public class BookmarkInfo
    {
        public Guid UserIdentifier { get; set; }

        public Guid ProductIdentifier { get; set; }
    }
}
