using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Core.Models
{
    public class RescueVehicleLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RescueVehicleId { get; set; }
        [Required]
        [Column(TypeName = "geography")]
        public Point Location { get; set; }
        public string? Address { get; set; }
        public bool Active { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;

        [ForeignKey("RescueVehicleId")]
        public RescueVehicle RescueVehicle { get; set; }
    }
}
