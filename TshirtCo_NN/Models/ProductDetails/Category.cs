using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TshirtCo_NN.Controllers;

namespace TshirtCo_NN.Models
{
    public class Category
    {
        /// <summary>
        /// constructor to create a new list of products for the categories
        /// </summary>
        public Category()
        {
            Products = new List<Product>();
        }

        /// <summary>
        /// primary key to identify the category
        /// </summary>
        [Key]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// name to be displayed for the category
        /// </summary>
        [Display(Name = "category")]
        public string CategoryName { get; set; }

        /// <summary>
        /// collection of products from the products class
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }
    }
}
