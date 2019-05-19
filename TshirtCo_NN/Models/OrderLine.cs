using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class OrderLine
    {
        public Guid OrderLineId { get; set; }
        public Guid ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double OrderLineTotal { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
