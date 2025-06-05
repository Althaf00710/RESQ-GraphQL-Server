using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CivilianTypeRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CivilianTypeId { get; set; }
        public int CivilianId { get; set; }
        [Required]
        public string proofImage { get; set; }



        [ForeignKey(nameof(CivilianTypeId))]
        public CivilianType CivilianType { get; set; }

        [ForeignKey(nameof(CivilianId))]
        public Civilian Civilian { get; set; }
    }
}
