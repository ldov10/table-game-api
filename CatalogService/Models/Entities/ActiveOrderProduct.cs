using System;

namespace CatalogService.Models.Entities
{
    public class ActiveOrderProduct : BaseEntity
    {
        public Guid ProductIdentifier { get; set; }

        public Guid OrderIdentifier { get; set; }
    }
}
