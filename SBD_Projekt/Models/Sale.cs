namespace SBDProjekt.Models
{
    using SBDProjekt.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 60 characters")]
        public string Name { get; set; }

        [BeforeEndDate(EndDatePropertyName = nameof(EndDate), ErrorMessage = "End date cannot be before start date")]
        public DateTime StartDate { get; set; }

        [AfterStartDate(StartDatePropertyName = nameof(StartDate), ErrorMessage = "Start date cannot be after end date")]
        public DateTime EndDate { get; set; }

        public ICollection<Product> DiscountedProducts { get; set; }
    }
}
