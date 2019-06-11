using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Colour
    {
        /// <summary>
        /// constructor to create a new list of products & designs for the colours
        /// </summary>
        public Colour()
        {
            Products = new List<Product>();
            Designs = new List<Design>();
        }

        /// <summary>
        /// primary key to identify the colour
        /// </summary>
        [Key]
        public Guid ColourId { get; set; }

        /// <summary>
        /// name to be displayed for the colour
        /// </summary>
        [Display(Name = "colour")]
        public string ColourName { get; set; }

        /// <summary>
        /// collection of products from the products class
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// collection of designs from the design class
        /// </summary>
        public virtual ICollection<Design> Designs { get; set; }
    }
}
