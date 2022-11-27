using Entities.Concrete;
using Shared.Data.Abstract;

namespace Data.Abstract
{
    public interface ICategoryRepository : IEntityRepository<Category>
    {
    }
}
