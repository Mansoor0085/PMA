using ProductManagementApp.Models;

namespace ProductManagementApp.Repository
{
    public interface IProductRepository
    {
        public List<Product> GetProducts();

        public Product GetProductById(int id);

        public void CreateProduct(Product product);

        public void UpdateProduct(Product product);

        public void DeleteProduct(Product product);
    }
}
