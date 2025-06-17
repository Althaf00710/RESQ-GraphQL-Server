
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class EmergencyToCivilian
    {
        public int Id { get; set; }
        public int CivilianStatusId { get; set; }
        public int EmergencyCategoryId { get; set; }

        [ForeignKey(nameof(EmergencyCategoryId))]
        public EmergencyCategory EmergencyCategory { get; set; }
        [ForeignKey(nameof(CivilianStatusId))]
        public CivilianStatus CivilianStatus { get; set; }
    }
}
