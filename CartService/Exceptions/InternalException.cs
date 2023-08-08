using System;

namespace CartService.Exceptions
{
    public class InternalException : Exception
    {
        public InternalException(string message = "") : base(message) { }
    }
}
