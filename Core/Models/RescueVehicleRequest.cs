using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Models
{
    public class RescueVehicleRequest
    {
        [Key]
        public int Id { get; set; }
        public int CivilianId { get; set; }
        [Required]
        public int EmergencyCategoryId { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Status { get; set; }
        public string? ProofImageURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



        [ForeignKey(nameof(CivilianId))]
        public EmergencyCategory EmergencyCategory { get; set; }
        public Civilian Civilian { get; set; }
        public ICollection<RescueVehicleAssignment> RescueVehicleAssignments { get; set; }
    }
}
