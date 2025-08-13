using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Core.Models
{
    public class CivilianLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CivilianId { get; set; }
        [Required]
        [Column(TypeName = "geography")]
        public Point Location { get; set; }
        public string? Address { get; set; }
        public bool Active { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
         

        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
    }
}
