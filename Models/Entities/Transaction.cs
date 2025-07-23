using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace week2_Task.Models.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Payment")]
        public Guid PaymentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionReference { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProcessedAmount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string Notes { get; set; }

        public Payment Payment { get; set; }

        public Transaction() { }

        public Transaction(Guid paymentId, string transactionReference, decimal processedAmount, string description = null, string notes = null)
        {
            PaymentId = paymentId;
            TransactionReference = transactionReference;
            ProcessedAmount = processedAmount;
            Description = description;
            Notes = notes;
            TransactionDate = DateTime.UtcNow;
        }
    }
}



