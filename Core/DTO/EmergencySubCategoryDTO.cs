using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class EmergencySubCategoryCreateInput
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int EmergencyCategoryId { get; set; }
    }

    public class EmergencySubCategoryUpdateInput
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


}
