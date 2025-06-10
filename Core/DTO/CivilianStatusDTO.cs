using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CivilianStatusCreateInput
    {
        public string Role { get; set; } = null!;
    }

    public class CivilianStatusUpdateInput
    {
        public string? Role { get; set; } 
    }
}
