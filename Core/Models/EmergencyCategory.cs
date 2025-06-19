
namespace Core.Models
{
    public class EmergencyCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }

        public ICollection<EmergencyToVehicle> EmergencyToVehicles { get; set; }
        public ICollection<EmergencyToCivilian> EmergencyToCivilians { get; set; }
    }
}
