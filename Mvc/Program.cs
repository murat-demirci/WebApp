using Data.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Mvc.AutoMapper.Profiles;
using Mvc.Helpers.Abstract;
using Mvc.Helpers.Concrete;
using Services.AutoMapper.Profiles;
using Services.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddJsonOptions(opt =>
    {
        //enum degerleri json formatinda dogru islenmesi icin
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //enum degerleri sayi olarak alir eger icine jsonnamepolicy.camelcase eklenirse yazi olarak
        //category controller da islemler yapildiginda front ende model donulecek
        //bu modelin js tarafinda da anlailmasi icin json formatina donusturulmesi gerek
        //jsonserializer
        //json formati icerisinde farkli objeler varsa burdan sorun cikmamsi icin,
        //(bug olma ihtimaline karsi controller sinifina da eklenir)
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    }).AddNToastNotifyToastr();//mvc uygulamasi oldugu belirtilir
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddServerSideBlazor();
builder.Services.AddSession();
builder.Services.AddDbContext<dContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});
builder.Services.LoadMyServices();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile), typeof(UserProfile), typeof(ViewModelsProfile));
builder.Services.ConfigureApplicationCookie(opt =>
{
    //cookie
    //login ve logout sayfalarinin adresi
    opt.LoginPath = new PathString("/Identity/Login/Index");
    opt.LogoutPath = new PathString("/Identity/User/Logout");
    opt.Cookie = new CookieBuilder
    {
        Name = "BlogSiteAdmin",
        HttpOnly = true,
        //herhangi bir kullanicinin cookie bilgimize ersimemesi icin
        SameSite = SameSiteMode.Strict
        ,//CSRF saldirisini onlemek icin
        //samesitemode.strict sadece bizim sitemizden gelen cookileri onaylar
        SecurePolicy = CookieSecurePolicy.Always
        //cookienin guvenligi
        //gercek projelerde herzaman https olarak alir
    };
    opt.SlidingExpiration = true;
    //kullaniciya zaman tanir cooki bilgileri sabit kalir
    //verilen sure sonunda cookie bilgileri sifirlanir
    opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
    opt.AccessDeniedPath = new PathString("/Identity/AccessDenied/Unauthorized");
    //sisteme giris yapmis fakat yetkisi olmayan alana girmeye calisan kullanicilar icin
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePages();//404sayfasina yonlendirir
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseStaticFiles();//css,resim,javascript dosyalari icin
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();//kullanici kontrolu
app.UseAuthorization();//yetki kontrolu
app.UseNToastNotify();
app.UseEndpoints(end =>
{
    end.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
    end.MapAreaControllerRoute(
        name: "Identity",
        areaName: "Identity",
        pattern: "Identity/{controller=Login}/{action=Index}/{id?}"
    );
    end.MapDefaultControllerRoute();
    //varsayilan olarak home controller ve index e gider
});
//farkli alanlarin gosterilmesi icin
//Admin area icindeki article index icine gidersek tablo gozukur ama direk article index e gidersek kullanici icin goruntu olur
//tek area admin oldugu icin tek route
//auto mapper microsoft extension depencyinjection paketi ve
//frontend icin viewlarda degisiklik yapinca gormek icin (anlik gostrerim) razor run time compilation



app.MapRazorPages();
app.Run();
