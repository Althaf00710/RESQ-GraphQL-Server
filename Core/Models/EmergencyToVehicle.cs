
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class EmergencyToVehicle
    {
        public int Id { get; set; }
        public int EmergencyCategoryId { get; set; }
        public int VehicleCategoryId { get; set; }


        [ForeignKey(nameof(EmergencyCategoryId))]
        public EmergencyCategory EmergencyCategory { get; set; }
        [ForeignKey(nameof(VehicleCategoryId))]
        public RescueVehicleCategory RescueVehicleCategory { get; set; }
    }
}
