using System.ComponentModel.DataAnnotations;

namespace week2_Task.Models.DTOS
{
    public class NewUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        
        public string? PhoneNumber { get; set; }

        [Required]
        
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
