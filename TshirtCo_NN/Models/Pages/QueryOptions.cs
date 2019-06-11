using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models.Pages
{
    public class QueryOptions
    {
        /// <summary>
        /// public variables for querying
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string OrderProductName { get; set; }
        public bool DescendingOrder { get; set; }

        public string SearchProductName { get; set; }
        public string SearchTerm { get; set; }
    }
}
