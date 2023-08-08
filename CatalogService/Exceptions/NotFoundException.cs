using System;

namespace CatalogService.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "") : base(message) { }
    }
}
