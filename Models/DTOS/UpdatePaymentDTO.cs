using System.ComponentModel.DataAnnotations;
using week2_Task.Models.Entities.Enums;

namespace week2_Task.Models.DTOS
{
    public class UpdatePaymentDTO
    {

        public decimal Amount { get; set; }

       
        public string Currency { get; set; }

     
        public PaymentMethod Method { get; set; }

       
        public Status Status { get; set; }

        public DateTime? ProcessedDate { get; set; }
    }
}

