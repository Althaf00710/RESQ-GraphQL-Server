using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class RescueVehicleLocationInput
    {
        public int RescueVehicleId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
    }

    public class RescueVehicleLocationStatusInput
    {
        public bool Active { get; set; }
    }
}