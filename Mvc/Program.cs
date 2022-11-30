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

        //json formati icerisinde farkli objeler varsa burdan sorun cikmamsi icin,
        //(bug olma ihtimaline karsi controller sinifina da eklenir)
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });//mvc uygulamasi oldugu belirtilir
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); ;
builder.Services.AddServerSideBlazor();
builder.Services.LoadMyServices();
builder.Services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));
//category controller da islemler yapildiginda front ende model donulecek
//bu modelin js tarafinda da anlailmasi icin json formatina donusturulmesi gerek
//jsonserializer


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePages();//404sayfasina yonlendirir
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();//css,resim,javascript dosyalari icin
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(end => end.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"));//farkli alanlarin gosterilmesi icin
//Admin area icindeki article index icine gidersek tablo gozukur ama direk article index e gidersek kullanici icin goruntu olur
//tek area admin oldugu icin tek route
//auto mapper microsoft extension depencyinjection paketi ve
//frontend icin viewlarda degisiklik yapinca gormek icin (anlik gostrerim) razor run time compilation

app.UseEndpoints(end => end.MapDefaultControllerRoute());//varsayilan olarak home controller ve index e gider


app.MapRazorPages();

app.Run();
