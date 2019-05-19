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
        public Category()
        {
            Products = new List<Product>();
        }

        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        //list of products
        public virtual ICollection<Product> Products { get; set; }
    }
}
