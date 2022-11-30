using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class User : IdentityUser<int>
    {
        //
        public ICollection<Article> Articles { get; set; }
        public string UserPicture { get; set; }
    }
}
