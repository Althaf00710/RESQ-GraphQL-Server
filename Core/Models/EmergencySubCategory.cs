

namespace Core.Models
{
    public class EmergencySubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int EmergencyCategoryId { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation property
        public EmergencyCategory EmergencyCategory { get; set; }
        public ICollection<FirstAidDetail> FirstAidDetails { get; set; }

    }
}
