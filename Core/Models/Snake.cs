﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Snake
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ScientificName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string VenomType { get; set; } // e.g., neurotoxic, hemotoxic, cytotoxic, non-venomous
        public string ImageUrl { get; set; }
    }
}
