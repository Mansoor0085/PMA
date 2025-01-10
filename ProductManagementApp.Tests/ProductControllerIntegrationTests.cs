using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductManagementApp.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ProductManagementApp.Tests
{
    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        [Fact]
        public async Task GetProducts_ShouldReturnOkWithListOfProducts()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var response = await _client.GetAsync(requestUri:"/api/Products");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            products.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public async Task GetProduct_ValidId_ShouldReturnOkWithProduct()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var validId = 1;

            var response = await _client.GetAsync(requestUri: $"/api/Products/id?id={validId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var product = await response.Content.ReadFromJsonAsync<Product>();
            product.Should().NotBeNull();
            product.Id.Should().Be(validId);
        }

        [Fact]
        public async Task GetProduct_InvalidId_ShouldReturnNotFound()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var invalidId = 9999;

            var response = await _client.GetAsync($"/api/Products/id?id={invalidId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostProduct_ValidData_ShouldReturnNoContent()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var newProduct = new Product
            {
                Name = "Mobile",
                Price = 10000M,
                Quantity = 100
            };

            var response = await _client.PostAsJsonAsync("/api/Products", newProduct);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task PutProduct_ValidData_ShouldReturnNoContent()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product",
                Price = 19999M,
                Quantity = 100
            };

            var response = await _client.PutAsJsonAsync("/api/Products", updatedProduct);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteProduct_ValidId_ShouldReturnNoContent()
        {
            var factory = new WebApplicationFactory<Program>();
            HttpClient _client = factory.CreateClient();
            var validId = 1;

            var response = await _client.DeleteAsync($"/api/Products/{validId}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }


}