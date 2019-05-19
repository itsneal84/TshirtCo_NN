using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Customer : ApplicationUser
    {
        public Guid CustomerId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
