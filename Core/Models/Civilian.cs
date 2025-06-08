
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.models;
using Core.Models;

namespace Core.Models
{
    public class Civilian
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NicNumber { get; set; }
        public int TypeId { get; set; } // "Traffic Police", "Doctor"
        public DateTime JoinedDate { get; set; }



        [ForeignKey(nameof(TypeId))]
        public CivilianType CivilianType { get; set; }
        public ICollection<CivilianLocation> CivilianLocations { get; set; }
        public ICollection<RescueVehicleRequest> RescueVehicleRequests { get; set; }
        public ICollection<CivilianTypeRequest> CivilianTypeRequests { get; set; }
    }
}
