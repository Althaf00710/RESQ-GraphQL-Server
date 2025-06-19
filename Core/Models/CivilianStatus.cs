using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class CivilianStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<CivilianStatusRequest> CivilianStatusRequests { get; set; }
        public ICollection<Civilian> Civilians { get; set; }
        public ICollection<EmergencyToCivilian> EmergencyToCivilians { get; set; }

    }
}
