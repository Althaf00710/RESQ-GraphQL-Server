using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class RescueVehicleCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<RescueVehicle> RescueVehicles { get; set; }
        public ICollection<EmergencyToVehicle> EmergencyToVehicles { get; set; }
    }
}
