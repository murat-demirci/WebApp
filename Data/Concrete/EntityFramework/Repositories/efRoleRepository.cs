using Data.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete.Dapper;

namespace Data.Concrete.EntityFramework.Repositories
{
    public class efRoleRepository : efEntityRepositoryBase<Role>, IRoleRepository
    {
        public efRoleRepository(DbContext context) : base(context)
        {

        }
    }
}
