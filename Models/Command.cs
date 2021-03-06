﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommanderAPI.Models
{
    public class Command
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        [MaxLength(500)]
        public string Line { get; set; }
        [Required]
        [MaxLength(100)]
        public string Platform { get; set; }
    }
}
