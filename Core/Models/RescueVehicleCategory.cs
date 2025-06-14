﻿using System.ComponentModel.DataAnnotations;
using Core.models;

namespace Core.Models
{
    public class RescueVehicleCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<RescueVehicle> RescueVehicles { get; set; }
    }
}
