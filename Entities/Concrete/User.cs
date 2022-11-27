using Shared.Entites.Abstract;

namespace Entities.Concrete
{
    public class User : EntityBase, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }//hashlenecegi icin byte tipinde
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Article> Articles { get; set; }
        public string UserPicture { get; set; }
        public string UserDescription { get; set; }
    }
}
