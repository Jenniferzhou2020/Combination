using MyFirstAngularNetApp.Server.Repository.Interface;
using MyFirstAngularNetApp.Server.Models;
using MyFirstAngularNetApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MyFirstAngularNetApp.Server.Repository.Interface;

namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    /// <summary>
    /// Repository to handle Category data CRUD
    /// Created by:
    /// Created Date: 
    /// </summary>
    public class CategoryRepository : GDCTRepository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbcontextfactory">DB Context Factory</param>
        /// <param name="logger"></param>
        public CategoryRepository(IDbContextFactory<GdctContext> dbcontextfactory, IAppLogger<Category> logger) : base(dbcontextfactory, logger)
        {
        }
        /// <summary>
        /// Get Categories's count
        /// </summary>
        /// <param name="predicate">lambda function method</param>
        /// <returns>Type: int</returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> CountCategoryAsync(Expression<Func<Category, bool>>? predicate)
        {

            try
            {
                using (var ctx = _dbcontextfactory.CreateDbContext())
                {
                    int result = (predicate == null) ? await ctx.Categories.CountAsync() : await ctx.Categories.CountAsync(predicate);
                    return result;
                }
             
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                _logger.LogError(ex, "An unexpected error occurred in MyFirstAngularNetApp.Server.Repository.Repositories.CategoryRepository()");
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }

}
