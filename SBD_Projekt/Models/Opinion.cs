namespace SBDProjekt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Opinion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Content text length must be between 1 and 1000 characters")]
        public string Content { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        public string ClientId { get; set; }
        public int ProductId { get; set; }

    }
}
