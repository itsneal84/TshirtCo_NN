using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Order
    {
        /// <summary>
        /// primary key to identify the order
        /// </summary>
        public Guid OrderId { get; set; }
        //public string CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLn1 { get; set; }
        public string AddressLn2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public Guid DesignId { get; set; }
        /// <summary>
        /// data annotations to set datatype for ordertotal
        /// </summary>
        [DataType(DataType.Currency)]
        public double OrderTotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool Dispatched { get; set; }
        public DateTime? DateShipped { get; set; }
        public string PaymentRef { get; set; }

        /// <summary>
        /// implements IEnumerable for orderlines so that it can be used in foreach loop
        /// </summary>
        public virtual IEnumerable<OrderLine> OrderLines { get; set; }
    }
}
