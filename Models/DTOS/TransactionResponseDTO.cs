namespace week2_Task.Models.DTOS
{
    
        public class TransactionResponseDTO
        {
            public int TransactionId { get; set; }
            public Guid PaymentId { get; set; }
            public string TransactionReference { get; set; }
            public decimal ProcessedAmount { get; set; }
            public string Description { get; set; }
            public string Notes { get; set; }
            public DateTime TransactionDate { get; set; }
        }
    }

