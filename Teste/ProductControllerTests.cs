using Delivery.Controllers;
using Delivery.Dtos.Product;
using Delivery.Models;
using Delivery.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Teste
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfProducts()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var products = new List<Product> { new() { Id = 1 }, new() { Id = 2 } };
            mockService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, ((List<Product>)returnedProducts).Count);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { Id = 1 };
            mockService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.GetProductByIdAsync(99)).ReturnsAsync((Product)null);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtAction_WithProduct()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var product = new Product { Id = 1 };
            mockService.Setup(s => s.AddProductAsync(product)).ReturnsAsync(product);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Add(product);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(product, createdAt.Value);
            Assert.Equal(nameof(controller.GetById), createdAt.ActionName);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.DeleteProductAsync(1)).ReturnsAsync(true);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotFound()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(s => s.DeleteProductAsync(99)).ReturnsAsync(false);
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UploadImage_ReturnsBadRequest_WhenNoFile()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.UploadImage(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Nenhum arquivo enviado.", badRequest.Value);
        }

        [Fact]
        public async Task UploadImage_ReturnsOk_WhenFileIsValid()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            var controller = new ProductController(mockService.Object);

            var fileMock = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns((Stream stream, System.Threading.CancellationToken token) =>
            {
                ms.CopyTo(stream);
                return Task.CompletedTask;
            });

            // Act
            var result = await controller.UploadImage(fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}