using System.ComponentModel.DataAnnotations;
using Core.models;

namespace Core.Models
{
    public class RescueVehicleType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<RescueVehicle> RescueVehicles { get; set; }
    }
}
