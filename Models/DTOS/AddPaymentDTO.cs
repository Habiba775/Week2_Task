using System.ComponentModel.DataAnnotations;
using week2_Task.Models.Entities.Enums;

namespace week2_Task.Models.DTOS
{
    public class AddPaymentDTO
    {
       
        public Guid MerchantId { get; set; }

       
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        
        public PaymentMethod Method { get; set; }

       
        public Status Status { get; set; }
    }
}
