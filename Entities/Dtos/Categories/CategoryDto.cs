using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos.Categories
{
    public class CategoryDto : DtoGetBase
    {
        public Category Category { get; set; }
    }
}
