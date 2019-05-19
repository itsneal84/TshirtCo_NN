using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Design
    {
        [Key]
        public Guid DesignId { get; set; }

        public string DesignName { get; set; }
        public string GarmentColourForPrint { get; set; }
        //[NotMapped]
        //public ICollection<Product> Products { get; set; }
    }
}
