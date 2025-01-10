using Microsoft.AspNetCore.Mvc;
using ProductManagementApp.Models;
using ProductManagementApp.Repository;
using ProductManagementApp.Service;


namespace ProductManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or a not found response if the product does not exist.</returns>
        [HttpGet("id")]
        public IActionResult GetProduct(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound(new { Message = "Product not found. " });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with ID {id}", id);
                return StatusCode(500, new { Message = "An error occurred while fetching the product. " });
            }
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>No content response if the product is created successfully, or a bad request response if validation fails.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _productService.CreateProduct(product);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">The updated product details.</param>
        /// <returns>No content response if the product is updated successfully, or a bad request response if validation fails.</returns>
        [HttpPut] 
        public ActionResult Put([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _productService.UpdateProduct(product);
            return NoContent(); 
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content response if the product is deleted successfully.</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent(); 
        }
    }
}
