using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TshirtCo_NN.Data;

namespace TshirtCo_NN.Models.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        /// <summary>
        /// private call to database
        /// </summary>
        private ApplicationDbContext context;

        /// <summary>
        /// constructor to set the database
        /// </summary>
        /// <param name="_context"></param>
        public OrdersRepository(ApplicationDbContext _context) => context = _context;

        /// <summary>
        /// implements IEnumerable for orders from database so that it can be used in foreach loop
        /// </summary>
        public IEnumerable<Order> Orders => context.Orders.Include(o => o.OrderLines).ThenInclude(l => l.Product);

        /// <summary>
        /// method to add an order to the database
        /// </summary>
        /// <param name="order"></param>
        public void AddOrder(Order order)
        {
            context.Orders.Add(order);
            //UpdateStock(order.ProductId);
            context.SaveChanges();
        }

        /// <summary>
        /// method to delete an order from the database
        /// </summary>
        /// <param name="order"></param>
        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        /// <summary>
        /// method to get order from the database using the passed in id
        /// </summary>
        /// <param name="key"></param>
        /// <returns>an order</returns>
        public Order GetOrder(Guid key) => context.Orders.Include(o => o.OrderLines).First(o => o.OrderId == key);

        /// <summary>
        /// method to update an order in the database
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(Order order)
        {
            context.Orders.Update(order);
            context.SaveChanges();
        }

        //not used
        //private void GetTotal(OrderLine orderLine)
        //{
        //}

        //not needed
        public void UpdateStock(Guid id)
        {
            var product = context.Products;
            var order = context.Orders;


        }
    }
}
