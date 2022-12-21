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

        //public async Task<Category> GetById(int categoryId)
        //{
        //    return await dContext.Categories.SingleOrDefault(c => c.ID == categoryId);
        //}
        //private dContext dContext
        //{
        //    get
        //    {
        //        return _context as dContext;
        //    }
        //}
        //efrepository icindeki dbcontexti kullanima ornek
    }
}
