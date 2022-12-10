using Entities.Concrete;
using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class UserListDto : DtoGetBase
    {
        public IList<User> Users { get; set; }
    }
}
