﻿using System;

namespace UserService.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "") : base(message) { }
    }
}
