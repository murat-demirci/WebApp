using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class CategoryDto : DtoGetBase
    {
        public Category Category { get; set; }
    }
}
