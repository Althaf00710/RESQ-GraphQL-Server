using System.ComponentModel.DataAnnotations;
using Core.models;

namespace Core.Models
{
    public class CivilianStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }

        public ICollection<CivilianStatusRequest> CivilianTypeRequests { get; set; }
        public ICollection<Civilian> Civilians { get; set; }

    }
}
