using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class AssignmentCreateInput
    {
        public int RescueVehicleRequestId { get; set; }
        public int RescueVehicleId { get; set; }
    }

    public class AssignmentUpdateInput
    {
        public string Status { get; set; }
    }
}
