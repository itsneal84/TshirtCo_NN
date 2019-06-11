using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models.Pages;

namespace TshirtCo_NN.Models.Repository
{
    /// <summary>
    /// DataRepository implements IRepository
    /// </summary>
    public class DataRepository : IRepository
    {
        /// <summary>
        /// private call to database
        /// </summary>
        private ApplicationDbContext context;

        /// <summary>
        /// constructor to set the database
        /// </summary>
        /// <param name="_context"></param>
        public DataRepository(ApplicationDbContext _context) => context = _context;
        /// <summary>
        /// implements IEnumerable for products from database so that it can be used in foreach loop via an array list
        /// </summary>
        public IEnumerable<Product> Products => context.Products.Include(p => p.Categories).ToArray();

        /// <summary>
        /// method for pagination of products (not used)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public PagedList<Product> GetProducts(QueryOptions options, Guid category)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Categories);
            if (category != null)
            {
                query = query.Where(p => p.CategoryId == category);
            }
            return new PagedList<Product>(query, options);
        }

        /// <summary>
        /// method to add a product to the database
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        /// <summary>
        /// method to delete a product from the database
        /// </summary>
        /// <param name="product"></param>
        public void Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        /// <summary>
        /// method to get a product from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>a product</returns>
        public Product GetProduct(Guid key) =>
            context.Products.Include(p => p.Categories).First(p => p.ProductId == key);

        /// <summary>
        /// method to update all aspects of a product & save to the database
        /// </summary>
        /// <param name="products"></param>
        public void UpdateAll(Product[] products)
        {
            Dictionary<Guid, Product> data = products.ToDictionary(p => p.ProductId);
            IEnumerable<Product> baseline = context.Products.Where(p => data.Keys.Contains(p.ProductId));
            foreach (var dbProduct in baseline)
            {
                Product productRequest = data[dbProduct.ProductId];
                dbProduct.ProductName = productRequest.ProductName;
                dbProduct.Categories = productRequest.Categories;
                dbProduct.StockLvl = productRequest.StockLvl;
                dbProduct.Colours.ColourName = productRequest.Colours.ColourName;
                dbProduct.Small = productRequest.Small;
                dbProduct.Medium = productRequest.Medium;
                dbProduct.Large = productRequest.Large;
                dbProduct.XLarge = productRequest.XLarge;
                dbProduct.Price = productRequest.Price;
                dbProduct.Image = productRequest.Image;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// method to update a product in the database
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            Product prod = context.Products.Find(product.ProductId);
            prod.ProductName = product.ProductName;
            prod.CategoryId = product.CategoryId;
            prod.StockLvl = product.StockLvl;
            prod.Colours.ColourName = product.Colours.ColourName;
            prod.Small = product.Small;
            prod.Medium = product.Medium;
            prod.Large = product.Large;
            prod.XLarge = product.XLarge;
            prod.Price = product.Price;
            prod.Image = product.Image;
            context.SaveChanges();
        }

        /// <summary>
        /// method to get a product from the database
        /// </summary>
        /// <param name="options"></param>
        /// <param name="category"></param>
        /// <returns>a product</returns>
        public PagedList<Product> GetProduct(QueryOptions options, Guid category)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Categories);
            if (category != null)
            {
                query = query.Where(p => p.CategoryId == category);
            }
            return new PagedList<Product>(query, options);
        }
    }
}
