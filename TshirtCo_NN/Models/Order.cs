using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductColour { get; set; }
        public string ProductSize { get; set; }
        public Guid DesignId { get; set; }
        [DataType(DataType.Currency)]
        public double OrderTotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? DateShipped { get; set; }
        public string PaymentDetails { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual Customer Customers { get; set; }
    }
}
