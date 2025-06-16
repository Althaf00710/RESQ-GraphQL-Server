using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class RescueVehicleCreateInput
    {
        public string PlateNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RescueVehicleCategoryId { get; set; } 
    }

    public class RescueVehicleUpdateInput
    {
        public string? PlateNumber { get; set; } 
        public string? Password { get; set; }
        public string? Status { get; set; }
    }
}
