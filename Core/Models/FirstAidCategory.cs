using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Core.models
{
    public class FirstAidCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }

        public ICollection<FirstAid> FirstAids { get; set; }
    }
}
