using System.ComponentModel.DataAnnotations;

namespace SBDProjekt.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 60 characters")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description length must be between 3 and 1000 characters")]
        public string Description { get; set; }
    }
}
