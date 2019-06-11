using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models.Pages
{
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// method for pagination (not currently used)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="options"></param>
        public PagedList(IQueryable<T> query, QueryOptions options = null)
        {
            CurrentPage = options.CurrentPage;
            PageSize = options.PageSize;
            Options = options;

            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.OrderProductName))
                {
                    query = Order(query, options.OrderProductName,
                        options.DescendingOrder);
                }
                if (!string.IsNullOrEmpty(options.SearchProductName)
                        && !string.IsNullOrEmpty(options.SearchTerm))
                {
                    query = Search(query, options.SearchProductName,
                        options.SearchTerm);
                }
            }

            TotalPages = query.Count() / PageSize;
            AddRange(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
        }

        /// <summary>
        /// public variables for pagination
        /// </summary>
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public QueryOptions Options { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// query method for searching products
        /// </summary>
        /// <param name="query"></param>
        /// <param name="productName"></param>
        /// <param name="searchTerm"></param>
        /// <returns>product</returns>
        private static IQueryable<T> Search(IQueryable<T> query, string productName,
                string searchTerm)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var source = productName.Split('.').Aggregate((Expression)parameter,
                Expression.Property);
            var body = Expression.Call(source, "Contains", Type.EmptyTypes,
                Expression.Constant(searchTerm, typeof(string)));
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return query.Where(lambda);
        }

        /// <summary>
        /// query method to search for orders
        /// </summary>
        /// <param name="query"></param>
        /// <param name="productName"></param>
        /// <param name="desc"></param>
        /// <returns>order</returns>
        private static IQueryable<T> Order(IQueryable<T> query, string productName,
                bool desc)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var source = productName.Split('.').Aggregate((Expression)parameter,
                Expression.Property);
            var lambda = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(T),
                source.Type), source, parameter);
            return typeof(Queryable).GetMethods().Single(
                      method => method.Name == (desc ? "OrderByDescending"
                                  : "OrderBy")
                      && method.IsGenericMethodDefinition
                      && method.GetGenericArguments().Length == 2
                      && method.GetParameters().Length == 2)
                  .MakeGenericMethod(typeof(T), source.Type)
                  .Invoke(null, new object[] { query, lambda }) as IQueryable<T>;
        }
    }
}
