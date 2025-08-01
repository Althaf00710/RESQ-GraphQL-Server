using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

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

    public class RescueVehicleLogin
    {
        public string JwtToken { get; set; } = null!;
        public RescueVehicle RescueVehicle { get; set; } = null!;
    }
}
