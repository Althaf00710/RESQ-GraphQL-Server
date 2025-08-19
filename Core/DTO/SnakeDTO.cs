using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.Models;

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

    public class SnakePredicted
    {
        public Snake Snake { get; set; } = new Snake();
        public double Prob { get; set; }
    }
}