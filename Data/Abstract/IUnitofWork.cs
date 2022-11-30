namespace Data.Abstract
{
    //unitofwork yapisi:
    //article ve user imizi degistirmek icin bu nesneleri kullanilidigi yerde tek tek newlemek gerekir
    //unitofwork ile bunlarin hepsine unitofwork u newleyerek erisebiliriz
    //tum repositorylerin tek bir yerden yonetilmesi
    //Ayrica transactions ile de tum islemler ayni anda save edilir
    //bir hata durumunda tum islemler iptal edilir
    public interface IUnitofWork : IAsyncDisposable//contexti dispose eder
    {
        IArticleRepository Articles { get; }//unitofwork.Articles ile ulasilabilir
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }//_unitofwork.categories.addAsync() kullanim ornegi
        //ayrica save metodu lazim veritabini islemesi icin
        //ornek:
        //_unitofwork.categories.addAsync(category)
        //_unitofwork.users.addAsync(user)
        //saveAsync()
        Task<int> SaveAsync();

    }
}
