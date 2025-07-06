using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class SnakeCreateInput
    {
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string Description { get; set; }
        public string VenomType { get; set; } // e.g., neurotoxic, hemotoxic, cytotoxic, non-venomous
    }

    public class SnakeUpdateInput
    {
        public string? Name { get; set; }
        public string? ScientificName { get; set; }
        public string? Description { get; set; }
        public string? VenomType { get; set; } // e.g., neurotoxic, hemotoxic, cytotoxic, non-venomous
    }
}
