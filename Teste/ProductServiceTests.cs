using Xunit;
using Moq;
using Delivery.Services;
using Delivery.Repositories.Interfaces;
using Delivery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teste
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetAllProductsAsync_ReturnsListOfProducts()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Product> {
                    new Product { Id = 1, Name = "Produto 1" },
                    new Product { Id = 2, Name = "Produto 2" }
                });
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, System.Linq.Enumerable.Count(result));
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenFound()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Produto 1" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.GetProductByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductAsync_CallsRepositoryAndReturnsProduct()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            var product = new Product { Id = 1, Name = "Produto 1" };
            mockRepo.Setup(r => r.AddAsync(product)).ReturnsAsync(product);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.AddProductAsync(product);

            // Assert
            mockRepo.Verify(r => r.AddAsync(product), Times.Once);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task DeleteProductAsync_CallsRepositoryAndReturnsTrue_WhenSuccess()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.DeleteProductAsync(1);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<ProductService>>();
            var service = new ProductService(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await service.DeleteProductAsync(99);

            // Assert
            Assert.False(result);
        }
    }
}