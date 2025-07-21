using System.ComponentModel.DataAnnotations;

namespace week2_Task.Models.Entities
{
    public class Merchant
    {
        
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public Merchant(string name, string email, string? phone = null, string? address = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }

        public Merchant() { }
    }
}

  
