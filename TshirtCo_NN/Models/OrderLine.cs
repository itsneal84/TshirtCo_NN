using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class OrderLine
    {
        /// <summary>
        /// primary key to identify the orderline
        /// </summary>
        public Guid OrderLineId { get; set; }
        public double OrderLineTotal { get; set; }

        /// <summary>
        /// order id foreign key from order table
        /// </summary>
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        /// <summary>
        /// product id foreign key from product table
        /// </summary>
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Colour { get; set; }

        /// <summary>
        /// data annotations to set display name for small size
        /// </summary>
        [Display(Name = "small")]
        public int Small { get; set; }
        /// <summary>
        /// data annotations to set display name for medium size
        /// </summary>
        [Display(Name = "medium")]
        public int Medium { get; set; }
        /// <summary>
        /// data annotations to set display name for large size
        /// </summary>
        [Display(Name = "large")]
        public int Large { get; set; }
        /// <summary>
        /// data annotations to set display name for xlarge size
        /// </summary>
        [Display(Name = "xlarge")]
        public int XLarge { get; set; }

        public bool Dispatched { get; set; }
        public DateTime? DateShipped { get; set; }

        //public Guid ColourId { get; set; }
        public Colour Colours { get; set; }
    }
}
