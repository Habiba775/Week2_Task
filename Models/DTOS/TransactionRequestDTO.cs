using System.ComponentModel.DataAnnotations;

namespace week2_Task.Models.DTOS
{
    public class TransactionRequestDTO
    {
        public Guid PaymentId { get; set; }
        public string TransactionReference { get; set; }
        public string Description { get; set; }
        public decimal ProcessedAmount { get; set; }
        public string Notes { get; set; }
    }


}