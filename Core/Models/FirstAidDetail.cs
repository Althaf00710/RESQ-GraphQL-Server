using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models;

namespace Core.models
{
    public class FirstAidDetail
    {
        [Key]
        public int Id { get; set; }
        public int FirstAidId { get; set; }
        public string Point { get; set; }
        [Required]
        public int Order { get; set; }

        [ForeignKey("FirstAidId")]
        public FirstAid FirstAid { get; set; }
    }
}
