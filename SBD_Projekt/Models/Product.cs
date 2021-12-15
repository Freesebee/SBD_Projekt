namespace SBDProjekt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01, 99999.99")]
        public double Price { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 60 characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Details length must be between 3 and 1000 characters")]
        public string Details { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
        public ICollection<Client> ProductEnjoyers { get; set;}
        public ICollection<OrderedProduct> OrderedProducts { get; set; }
        public ICollection<Sale> Sales { get; set; }

    }
}
