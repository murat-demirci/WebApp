using Data.Abstract;
using Data.Concrete;
using Data.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstract;
using Services.Concrete;

namespace Services.Extensions
{
    //extensionlar mvc ve data katmaninin arasinda bulunur
    //katmanlar kendilerinden bir ust katmana ersimelidir
    //bu sebeble data katmani dogrudan mvc katmanina erismesi dogru degildir
    //service katamni bu iki katman arasinda kopru gorevi gorur
    //static olurlar
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<dContext>();
            serviceCollection.AddIdentity<User, Role>(opt =>
            {
                //sifre
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequiredUniqueChars = 1;//en az 1 ozel karakter sayi degeri
                opt.Password.RequireNonAlphanumeric = true;//bu ayar zorunlu olmasini saglar
                opt.Password.RequireUppercase = true;
                //kullanici
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters =
                "abcdefgğhiıjklmnopqrstuüvwxyzABCDEFGĞHIİJKLMNOPQRSTUÜVWXYZ0123456789-._@+";
                opt.SignIn.RequireConfirmedEmail = true;
                //kullanici icin izin verilen karakterler
                opt.Lockout.AllowedForNewUsers = false;
            }).AddEntityFrameworkStores<dContext>().AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);
            //migrations islemleri icin(identity)
            serviceCollection.AddScoped<IUnitofWork, UnitofWork>();
            //scoped: bir istekte bulundugunda ve bu islemlere baslandiginda bu islemlerin
            //butunu scope icerisine alinir ve yurutulur.
            //tum islemler bittikten sonra (site ile baglanti kesildiignde) scope da kendini kapatir
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();

            return serviceCollection;
            //burdan Program.cs dosyasina git
        }

    }
}
