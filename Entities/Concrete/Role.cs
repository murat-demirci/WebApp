using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete
{
    public class Role : IdentityRole<int>
    {
        //bir rol birden fazla kullanicida oldugu icin collection tipinde

    }
}
