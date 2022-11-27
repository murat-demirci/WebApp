using Data.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete.Dapper;

namespace Data.Concrete.EntityFramework.Repositories
{
    public class efCategoryRepository : efEntityRepositoryBase<Category>, ICategoryRepository
    {
        public efCategoryRepository(DbContext context) : base(context)
        {

        }
    }
}
