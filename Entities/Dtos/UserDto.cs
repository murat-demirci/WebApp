using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class UserDto : DtoGetBase
    {
        public User User { get; set; }
    }
}
