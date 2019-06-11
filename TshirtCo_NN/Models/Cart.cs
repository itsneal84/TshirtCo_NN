using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TshirtCo_NN.Data;

namespace TshirtCo_NN.Models
{
    public class Cart
    {
        /// <summary>
        /// private call to database
        /// </summary>
        private ApplicationDbContext _context;
        /// <summary>
        /// private list of orderlines
        /// </summary>
        private List<OrderLine> olList = new List<OrderLine>();
        //private List<Order> orderList = new List<Order>();

        /// <summary>
        /// method to get the selected product from the database and saved into the cart class
        /// </summary>
        /// <param name="p"></param>
        /// <returns>product to cart</returns>
        public Cart AddItem(Product p)
        {
            OrderLine orderLine = olList.Where(l => l.ProductId == p.ProductId).FirstOrDefault();

            //check if an orderline already exists
            if (orderLine != null)
            {
                //if it does check if the new product is the same as the old
                if (orderLine.ProductId == p.ProductId)
                {
                    //if it is update the old product
                    orderLine.Small = (orderLine.Small += p.Small);
                    orderLine.Medium = (orderLine.Medium += p.Medium);
                    orderLine.Large = (orderLine.Large += p.Large);
                    orderLine.XLarge = (orderLine.XLarge += p.XLarge);
                    orderLine.Quantity = (orderLine.Small + orderLine.Medium + orderLine.Large + orderLine.XLarge);
                }
                //if not add a new orderline
                else
                {
                    olList.Add(new OrderLine
                    {
                        ProductId = p.ProductId,
                        Product = p,
                        Name = p.ProductName,
                        Colour = p.Colour,
                        Small = p.Small,
                        Medium = p.Medium,
                        Large = p.Large,
                        XLarge = p.XLarge,
                        Quantity = (p.Small + p.Medium + p.Large + p.XLarge),
                        Price = p.Price
                    });
                }
            }
            //if not add a new orderline
            else
            {
                    olList.Add(new OrderLine
                    {
                        ProductId = p.ProductId,
                        Product = p,
                        Name = p.ProductName,
                        Colour = p.Colour,
                        Small = p.Small,
                        Medium = p.Medium,
                        Large = p.Large,
                        XLarge = p.XLarge,
                        Quantity = (p.Small + p.Medium + p.Large + p.XLarge),
                        Price = p.Price
                    });
            }

            return this;
        }

        /// <summary>
        /// remove the product from the cart class
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>empty cart</returns>
        public Cart RemoveItem(Guid productId)
        {
            //remove product from the cart if the ids match
            olList.RemoveAll(r => r.ProductId == productId);
            return this;
        }

        /// <summary>
        /// clear the list of orderlines
        /// </summary>
        public void Clear() => olList.Clear();

        public IEnumerable<OrderLine> OlList { get => olList; }
        //public IEnumerable<Order> OrderList { get => orderList; }
    }
}
