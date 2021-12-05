using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBDProjekt.Models
{
    public class DiscountedProduct
    {
        public int Discount { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public int Discount { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}