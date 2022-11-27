using Data.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Concrete.Dapper;

namespace Data.Concrete.EntityFramework.Repositories
{
    public class efCommentRepository : efEntityRepositoryBase<Comment>, ICommentRepository
    {
        public efCommentRepository(DbContext context) : base(context)
        {

        }
    }
}
