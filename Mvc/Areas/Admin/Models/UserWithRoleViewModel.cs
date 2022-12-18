using Entities.Concrete;

namespace Mvc.Areas.Admin.Models
{
    public class UserWithRoleViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
