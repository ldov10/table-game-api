﻿using System;

namespace CartService.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "") : base(message) { }
    }
}
