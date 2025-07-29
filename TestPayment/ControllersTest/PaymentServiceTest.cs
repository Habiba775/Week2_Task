using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using week2_Task.Models.Entities;

namespace TestPayment.ControllersTest
{
    public class PaymentServiceTests
    {

        private readonly Mock<IPaymentRepository> _repoMock;
        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _repoMock = new Mock<IPaymentRepository>();
            _service = new PaymentService(_repoMock.Object);
        }

        [Fact]
        public async Task GetPaymentDetails_ExistingId_ReturnsPayment()
        {
           
            var paymentId = Guid.NewGuid();
            var payment = new Payment { PaymentId = paymentId };
            _repoMock.Setup(r => r.GetByIdAsync(paymentId)).ReturnsAsync(payment);

            
            var result = await _service.GetPaymentDetails(paymentId);

            
            Assert.NotNull(result);
            Assert.Equal(paymentId, result.PaymentId);
        }
    }
}
