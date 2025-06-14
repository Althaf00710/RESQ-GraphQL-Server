using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CivilianLocationInput
    {
        public int CivilianId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Location { get; set; } = null!;
    }

    public class CivilianLocationStatusInput
    {
        public bool Active { get; set; }
    }
}