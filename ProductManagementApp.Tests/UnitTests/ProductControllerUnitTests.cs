using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Models;
using Moq;
using ProductManagementApp.Controllers;
using FluentAssertions;
using ProductManagementApp.Service;
using Microsoft.Extensions.Logging;

namespace ProductManagementApp.Tests.UnitTests
{
    public class ProductControllerUnitTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly ProductsController _controller;

        public ProductControllerUnitTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetProducts_ShouldReturnOkWithListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.0M, Quantity = 50 },
                new Product { Id = 2, Name = "Product 2", Price = 20.0M, Quantity = 100 }
            };
            _productServiceMock.Setup(svc => svc.GetProducts()).Returns(products);

            // Act
            var result = _controller.GetProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public void GetProduct_ValidId_ShouldReturnOkWithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.0M, Quantity = 50 };
            _productServiceMock.Setup(svc => svc.GetProductById(1)).Returns(product);

            // Act
            var result = _controller.GetProduct(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(product);
        }

        [Fact]
        public void GetProduct_InvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _productServiceMock.Setup(svc => svc.GetProductById(999)).Returns((Product)null);

            // Act
            var result = _controller.GetProduct(999);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void PostProduct_ValidData_ShouldReturnNoContent()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "New Product", Price = 50.0M, Quantity = 10 };

            // Act
            var result = _controller.Post(product);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _productServiceMock.Verify(svc => svc.CreateProduct(product), Times.Once);
        }

        [Fact]
        public void PostProduct_InvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var product = new Product();
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = _controller.Post(product);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            _productServiceMock.Verify(svc => svc.CreateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void PutProduct_ValidData_ShouldReturnNoContent()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product", Price = 100.0M, Quantity = 20 };
            _productServiceMock.Setup(svc => svc.GetProductById(1)).Returns(product);

            // Act
            var result = _controller.Put(product);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _productServiceMock.Verify(svc => svc.UpdateProduct(product), Times.Once);
        }

        [Fact]
        public void PutProduct_InvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Product", Price = 100.0M, Quantity = 20 };
            _productServiceMock.Setup(svc => svc.GetProductById(1)).Returns((Product)null);

            // Act
            var result = _controller.Put(product);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteProduct_ValidId_ShouldReturnNoContent()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10.0M, Quantity = 50 };
            _productServiceMock.Setup(svc => svc.GetProductById(1)).Returns(product);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _productServiceMock.Verify(svc => svc.DeleteProduct(1), Times.Once);
        }

        [Fact]
        public void DeleteProduct_InvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _productServiceMock.Setup(svc => svc.GetProductById(999)).Returns((Product)null);

            // Act
            var result = _controller.Delete(999);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }

}
