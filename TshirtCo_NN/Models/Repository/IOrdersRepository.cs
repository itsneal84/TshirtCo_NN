using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models.Repository
{
    public interface IOrdersRepository
    {
        /// <summary>
        /// implements IEnumerable for orders from order repository so that it can be used in foreach loop
        /// </summary>
        IEnumerable<Order> Orders { get; }
        Order GetOrder(Guid key);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
