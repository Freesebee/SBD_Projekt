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
        public double Price { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public ICollection<Client> ProductEnjoyers { get; set;}
        public ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
