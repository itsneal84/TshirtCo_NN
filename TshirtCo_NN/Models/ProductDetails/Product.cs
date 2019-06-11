using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TshirtCo_NN.Models
{
    public class Product
    {
        /// <summary>
        /// primary key to identify the product
        /// </summary>
        [Key]
        public Guid ProductId { get; set; }
        /// <summary>
        /// data annotations to set requirement & display name for approval
        /// </summary>
        [Required]
        [Display(Name = "product name")]
        public string ProductName { get; set; }
        /// <summary>
        /// data annotations to set requirement for price
        /// </summary>
        [Required]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        /// <summary>
        /// -- not used in final project replaced with sizes --
        /// data annotations to set display name for stock level
        /// </summary>
        [Display(Name = "quantity")]
        public int StockLvl { get; set; }
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

        /// <summary>
        /// location of the stored image for the product
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// category id foreign key from category table
        /// </summary>
        public Guid CategoryId { get; set; }

        public Category Categories { get; set; }

        /// <summary>
        /// colour id foreign key from colour table
        /// </summary>
        public Guid ColourId { get; set; }
        public Colour Colours { get; set; }
        public string Colour { get; set; }

        //public virtual  ICollection<OrderLine> OrderLines { get; set; }

        //public Guid DesignId { get; set; }
        public virtual Design Designs { get; set; }
    }
}