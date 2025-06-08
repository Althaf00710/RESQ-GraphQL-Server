using System.ComponentModel.DataAnnotations;
using Core.models;

namespace Core.Models
{
    public class CivilianType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<CivilianTypeRequest> CivilianTypeRequests { get; set; }
        public ICollection<Civilian> Civilians { get; set; }

    }
}
