using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBDProjekt.Models
{
    public class FavouriteProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}