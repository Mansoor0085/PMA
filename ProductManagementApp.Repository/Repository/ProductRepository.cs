using Newtonsoft.Json;
using ProductManagementApp.Models;
using System.Reflection;

namespace ProductManagementApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private string filepath = string.Empty;

        public ProductRepository()
        {
            filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data.json");
        }

        private List<Product> ProductsList()
        {
            if (!File.Exists(filepath))
            {
                return [];
            }
            var json = File.ReadAllText(filepath);
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }

        public Product GetProductById(int id)
        {
            var products = ProductsList();
            return products.Find(x => x.Id == id);
        }

        public List<Product> GetProducts()
        {
            return ProductsList();
        }

        public void CreateProduct(Product product)
        {
            var products = GetProducts();
            products.Add(product);
            SaveAllProducts(products);
        }


        public void UpdateProduct(Product product)
        {
            var products = GetProducts();

            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                var index = products.IndexOf(existingProduct);
                products[index] = product;
            }
            else
            {
                throw new Exception($"Product with ID {product.Id} not found.");
            }

            SaveAllProducts(products);
        }

        public void DeleteProduct(Product product)
        {
            var products = GetProducts();

            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                products.Remove(existingProduct);
                SaveAllProducts(products);
            }
            else
            {
                throw new Exception($"Product with ID {product.Id} not found.");
            }
        }

        private void SaveAllProducts(List<Product> products)
        {
            var updatedJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(filepath, updatedJson);
        }
    }
}
    