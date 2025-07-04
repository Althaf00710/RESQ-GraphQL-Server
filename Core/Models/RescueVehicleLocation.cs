﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class RescueVehicleLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RescueVehicleId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }

        [ForeignKey("RescueVehicleId")]
        public RescueVehicle RescueVehicle { get; set; }
    }
}
