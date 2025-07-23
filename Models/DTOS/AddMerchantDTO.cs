using System.ComponentModel.DataAnnotations;

namespace week2_Task.Models.DTOS
{
    public class AddMerchantDTO
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
