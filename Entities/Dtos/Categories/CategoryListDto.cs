using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos.Categories
{
    public class CategoryListDto : DtoGetBase
    {
        public IList<Category> Categories { get; set; }
    }
}
