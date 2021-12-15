namespace SBDProjekt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public ICollection<OrderedProduct> OrderedProduct { get; set; }
    }
}
