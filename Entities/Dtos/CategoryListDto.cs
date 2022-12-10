using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class CategoryListDto : DtoGetBase
    {
        public IList<Category> Categories { get; set; }
    }
}
