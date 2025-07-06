using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class RescueVehicleRequestCreateInput
    {
        [Required]
        public int CivilianId { get; set; }

        [Required]
        public int EmergencyCategoryId { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude { get; set; }

        public string? ProofImage { get; set; }
    }

    public class RescueVehicleRequestUpdateInput
    {
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
