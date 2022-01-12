using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBDProjekt.Models.ViewModels
{
    public class EditDiscountedProductViewModel
    {
        [Required]
        [Range(1, 99, ErrorMessage = "Discount must be a percentage between 1, 99")]
        public int Discount { get; set; }

        [Required]
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public IList<Sale> SaleList { get; set; }
    }
}
