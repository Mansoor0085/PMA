using ProductManagementApp.Models;

namespace ProductManagementApp.Service
{
    public interface IProductService
    {
        public List<Product> GetProducts();

        public Product GetProductById(int id);

        public void CreateProduct(Product product);

        public void UpdateProduct(Product product);

        public void DeleteProduct(int id);
    }
}
