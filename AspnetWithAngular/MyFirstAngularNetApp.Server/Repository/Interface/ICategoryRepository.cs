using System.Linq.Expressions;
using MyFirstAngularNetApp.Server.Models;

namespace MyFirstAngularNetApp.Server.Repository.Interface
{
    public interface ICategoryRepository : IGDCTRepository<Category>
    {
        Task<int> CountCategoryAsync(Expression<Func<Category, bool>>? predicate);
    }
}
