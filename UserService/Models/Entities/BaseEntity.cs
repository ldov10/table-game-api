﻿using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid Identifier { get; set; } = Guid.NewGuid();
    }
}
