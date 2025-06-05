using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.models;

namespace Core.Models
{
    public class FirstAid
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey(nameof(CategoryId))]
        public FirstAidCategory FirstAidCategory { get; set; }
        public ICollection<FirstAidDetail> FirstAidDetails { get; set; }
    }
}
