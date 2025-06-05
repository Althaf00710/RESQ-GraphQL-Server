using System.ComponentModel.DataAnnotations;

namespace Core.models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.Now;
        public DateTime? LastActive { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ProfilePicturePath { get; set; } // Optional
    }
}
