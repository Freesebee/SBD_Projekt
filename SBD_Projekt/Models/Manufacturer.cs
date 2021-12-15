namespace SBDProjekt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 60 characters")]
        public string Name { get; set; }
    }
}
