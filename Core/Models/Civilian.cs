
using System.ComponentModel.DataAnnotations;
using Core.models;
using Core.Models;

namespace Core.Models
{
    public class Civilian
    {
        [Key]
        public int Id { get; set; }
        [Required, Phone]
        public string PhoneNumber { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NicNumber { get; set; }
        public string type { get; set; } // "Traffic Police", "Doctor"
        public DateTime JoinedDate { get; set; }


        public ICollection<CivilianLocation> CivilianLocations { get; set; }
        public ICollection<RescueVehicleRequest> RescueVehicleRequests { get; set; }
        public ICollection<CivilianTypeRequest> CivilianTypeRequests { get; set; }
    }
}
