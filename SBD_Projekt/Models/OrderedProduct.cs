using System.ComponentModel.DataAnnotations;

namespace SBDProjekt.Models
{
    public class OrderedProduct
    {
        [Required]
        [Range(1, 999999, ErrorMessage = "Quantity must be between 1 and 999999")]
        public int Quantity { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}