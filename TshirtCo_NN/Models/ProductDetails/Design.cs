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
        /// <summary>
        /// constructor to create a new list of products for the designs
        /// </summary>
        public Design()
        {
            Products = new List<Product>();
        }

        /// <summary>
        /// primary key to identify the design
        /// </summary>
        [Key]
        public Guid DesignId { get; set; }

        /// <summary>
        /// name to be displayed for the category
        /// </summary>
        [Display(Name = "design")]
        public string DesignName { get; set; }

        /// <summary>
        /// colour of garment that the design can be printed on 
        /// </summary>
        public string GarmentColourForPrint { get; set; }

        /// <summary>
        /// path for the image uploaded of the design 
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// collection of products from the products class
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}
