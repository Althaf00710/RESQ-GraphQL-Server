using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CivilianStatusRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CivilianStatusId { get; set; }
        public int CivilianId { get; set; }
        [Required]
        public string proofImage { get; set; }
        public string status { get; set; } // "Pending", "Approved", "Rejected"


        [ForeignKey(nameof(CivilianStatusId))]
        public CivilianStatus CivilianType { get; set; }

        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
    }
}
