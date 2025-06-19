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
        public string Reason { get; set; }
        [Required]
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Status { get; set; }
        public string proofImage { get; set; }


        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
        public ICollection<RescueVehicleAssignment> RescueVehicleAssignments { get; set; }
    }
}
