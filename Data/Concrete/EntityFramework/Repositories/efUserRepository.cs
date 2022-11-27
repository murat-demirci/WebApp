using Data.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete.Dapper;

namespace Data.Concrete.EntityFramework.Repositories
{
    public class efUserRepository : efEntityRepositoryBase<User>, IUserRepository
    {
        public efUserRepository(DbContext context) : base(context)
        {

        }
    }
}
