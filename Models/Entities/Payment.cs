using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using week2_Task.Models.Entities.Enums;
using week2_Task.Migrations;





namespace week2_Task.Models.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        [ForeignKey("Merchant")]
        public Guid MerchantId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentMethod Method { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public Merchant Merchant { get; set; }
        public DateTime? ProcessedDate { get; set; }

        public Payment(Guid merchantId, decimal amount, string currency, PaymentMethod method, Status status)
        {

            Id = Guid.NewGuid();
            MerchantId = merchantId;
            Amount = amount;
            Currency = currency;
            Method = method;
            Status = status;
            CreatedDate = DateTime.UtcNow;
        }

        public Payment() { }
    }
}

