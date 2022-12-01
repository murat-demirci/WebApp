using Shared.Entites.Abstract;

namespace Entities.Dtos
{
    public class UserListDto : DtoGetBase
    {
        public IList<Entities.Concrete.User> Users { get; set; }
    }
}
