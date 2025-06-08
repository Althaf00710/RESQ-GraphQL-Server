using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CivilianCreateInput
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string NicNumber { get; set; } = null!;  
    }

    public class CivilianUpdateInput
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NicNumber { get; set; }
        public string? Type { get; set; } // "Traffic Police", "Doctor"
    }
}
