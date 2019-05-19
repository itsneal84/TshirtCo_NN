using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TshirtCo_NN.Models
{
    public class Product
    {
        public Product()
        {
            OrderLines = new List<OrderLine>();
        }

        [Key]
        public Guid ProductId { get; set; }
        [Required]
        [Display(Name = "product name")]
        public string ProductName { get; set; }
        [Required]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "quantity")]
        public int StockLvl { get; set; }
        [Required]
        public string Colour { get; set; }
        [Required]
        public string Size { get; set; }

        public Guid CategoryId { get; set; }
        [Required]
        public virtual Category Categories { get; set; }

        public virtual  ICollection<OrderLine> OrderLines { get; set; }

        //[ForeignKey("Design")]
        //public Guid DesignId { get; set; }

        //public Design Designs { get; set; }
        //[Required]
        ////public virtual ICollection<ProductImageFile> ProductImageFiles { get; set; }
        //public string ProductImage { get; set; }
    }
}