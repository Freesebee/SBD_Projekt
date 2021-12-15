using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBDProjekt.Models
{
    public class FavouriteProduct
    {
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public string ClientId { get; set; }
        public Client Client { get; set; }
    }
}