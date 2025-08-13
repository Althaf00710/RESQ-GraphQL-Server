using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;
using NetTopologySuite.Geometries;

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
        public string Address { get; set; }
        [Column(TypeName = "geography")]
        [Required]
        public Point Location {  get; set; }
        public string Status { get; set; }
        public string? ProofImageURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(EmergencyCategoryId))]
        public EmergencyCategory EmergencyCategory { get; set; }

        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
        public ICollection<RescueVehicleAssignment> RescueVehicleAssignments { get; set; }
    }
}
