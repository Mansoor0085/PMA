using System.ComponentModel.DataAnnotations;

namespace ProductManagementApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed.")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
