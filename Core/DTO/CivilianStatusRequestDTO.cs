using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CivilianStatusRequestCreateInput
    {
        public int CivilianStatusId { get; set; }
        public int CivilianId { get; set; }

    }

    public class CivilianStatusRequestUpdateInput
    {
        public string Status { get; set; } //"Approved", "Rejected"
    }
}
