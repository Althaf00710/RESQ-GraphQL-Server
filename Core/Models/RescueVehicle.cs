using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.models
{
    public class RescueVehicle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string Status { get; set; }
        [Required]
        public int RescueVehicleCategoryId { get; set; }


        [ForeignKey(nameof(RescueVehicleCategoryId))]
        public RescueVehicleCategory RescueVehicleCategory { get; set; }
        public ICollection<RescueVehicleLocation> RescueVehicleLocations { get; set; }
        public ICollection<RescueVehicleAssignment> RescueVehicleAssignment { get; set; }
    }
}
