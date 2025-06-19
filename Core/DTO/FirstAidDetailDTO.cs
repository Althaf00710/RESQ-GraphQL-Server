using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class FirstAidDetailCreateInput
    {
        public int EmergencySubCategoryId { get; set; }
        public string Point { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class FirstAidDetailUpdateInput
    {
        public string? Point { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
