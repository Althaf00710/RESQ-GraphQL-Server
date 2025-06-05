using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CivilianLocation
    {
        [Key]
        public int Id { get; set; }
        public int CivilianId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }


        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
    }
}
