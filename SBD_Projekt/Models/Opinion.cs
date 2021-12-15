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
        public string Content { get; set; }
        public int Rating { get; set; }
        
        public string ClientId { get; set; }
        public int ProductId { get; set; }

    }
}
