using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TshirtCo_NN.Models.Pages;

namespace TshirtCo_NN.Models.Repository
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }

        PagedList<Product> GetProduct(QueryOptions options, Guid category);

        Product GetProduct(Guid key);
        /// <summary>
        /// add new products to database
        /// </summary>
        /// <param name="product"></param>
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void UpdateAll(Product[] products);
        void Delete(Product product);
    }
}
