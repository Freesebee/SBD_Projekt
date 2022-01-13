using System.Collections.Generic;

namespace SBDProjekt.Models.ViewModels
{
    public class DetailsSaleViewModel : Sale
    {
        public DetailsSaleViewModel(Sale sale)
        {
            Id = sale.Id;
            Name = sale.Name;
            StartDate = sale.StartDate;
            EndDate = sale.EndDate;
            DiscountedProducts = sale.DiscountedProducts;
        }

        public ICollection<DiscountedProduct> Discounts { get; set; }
    }
}
