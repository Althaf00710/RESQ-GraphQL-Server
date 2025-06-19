using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.Models
{
    public class FirstAidDetail
    {
        [Key]
        public int Id { get; set; }
        public int EmergencySubCategoryId { get; set; }
        public string Point { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey("EmergencySubCategoryId")]
        public EmergencySubCategory EmergencySubCategory { get; set; }
    }
}
