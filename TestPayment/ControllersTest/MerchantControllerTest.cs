/*using week2_Task.Controllers;
using week2_Task.Models.Entities;
using week2_Task.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace week2_Task.Tests.Controllers
{
    public class MerchantControllerTests
    {
        // Helper method to create a new in-memory database with sample data
        private ApplicationDBContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // new DB for each test
                .Options;

            var context = new ApplicationDBContext(options);

            // Add test merchants with all required fields (Email is required)
            context.Merchants.AddRange(new List<Merchant>
            {
                new Merchant
                {
                    Id = Guid.NewGuid(),
                    Name = "Merchant One",
                    Email = "merchant1@example.com", // Required field
                      
                    Address = "Address 1"
                },
                new Merchant
                {
                    Id = Guid.NewGuid(),
                    Name = "Merchant Two",
                    Email = "merchant2@example.com", // Required field
                    
                    Address = "Address 2"
                }
            });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMerchants()
        {
            // Arrange
            var dbContext = GetDbContextWithData();
            var controller = new MerchantController(dbContext);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var merchants = Assert.IsAssignableFrom<IEnumerable<Merchant>>(okResult.Value);
            Assert.Equal(2, merchants.Count());
        }

        [Fact]
        public void GetMerchantById_ReturnsMerchant_WhenFound()
        {
            // Arrange
            var dbContext = GetDbContextWithData();
            var merchant = dbContext.Merchants.First();
            var controller = new MerchantController(dbContext);

            // Act
            var result = controller.GetMerchantById(merchant.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMerchant = Assert.IsType<Merchant>(okResult.Value);
            Assert.Equal(merchant.Id, returnedMerchant.Id);
        }

        [Fact]
        public void GetMerchantById_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            var dbContext = GetDbContextWithData();
            var controller = new MerchantController(dbContext);
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = controller.GetMerchantById(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Contains(nonExistentId.ToString(), message);
        }
    }
}

*/


