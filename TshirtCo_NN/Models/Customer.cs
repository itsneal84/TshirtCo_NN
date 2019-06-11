using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    /// <summary>
    /// customer class inherits from applicationuser
    /// </summary>
    public class Customer : ApplicationUser
    {
        /// <summary>
        /// variables to extend applicationuser functionality
        /// </summary>
        /// primary ket for customer
        public Guid CustomerId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
