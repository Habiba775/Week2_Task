using week2_Task.Models.Entities.Enums;

namespace week2_Task.Models.DTOS
{
    public class PaymentResponseDTO
    {

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentMethod Method { get; set; }
        public Status Status { get; set; }
        public Guid MerchantId { get; set; }
    }
}
