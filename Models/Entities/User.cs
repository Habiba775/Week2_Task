using System.ComponentModel.DataAnnotations;

namespace week2_Task.Models.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
       

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}