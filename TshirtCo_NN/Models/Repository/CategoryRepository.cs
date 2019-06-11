using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TshirtCo_NN.Data;
using TshirtCo_NN.Models.Pages;

namespace TshirtCo_NN.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// private call to database
        /// </summary>
        private ApplicationDbContext context;

        /// <summary>
        /// constructor to set the database
        /// </summary>
        /// <param name="_context"></param>
        public CategoryRepository(ApplicationDbContext _context) => context = _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        PagedList<Category> ICategoryRepository.GetCategories(QueryOptions options)
        {
            return new PagedList<Category>(context.Categories, options);
        }

        /// <summary>
        /// implements IEnumerable for categories from database so that it can be used in foreach loop
        /// </summary>
        public IEnumerable<Category> Categories => context.Categories;

        /// <summary>
        /// method to add a category to the database
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        /// <summary>
        /// method to update a category in the database
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }

        /// <summary>
        /// method to delete a category from the database
        /// </summary>
        /// <param name="category"></param>
        public void DeleteCategory(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }

    /// <summary>
    /// interface for the category repository (not used)
    /// </summary>
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
        PagedList<Category> GetCategories(QueryOptions options);

        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
