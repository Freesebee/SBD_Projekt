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
        public string Name { get; set; }
    }
}
