using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.models
{
    public class RescueVehicleAssignment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RescueVehicleRequestId { get; set; }
        [Required]
        public int RescueVehicleId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }


        [ForeignKey(nameof(RescueVehicleRequestId))]
        public RescueVehicleRequest RescueVehicleRequest { get; set; }
        [ForeignKey(nameof(RescueVehicleId))]
        public RescueVehicle RescueVehicle { get; set; }
    }
}
