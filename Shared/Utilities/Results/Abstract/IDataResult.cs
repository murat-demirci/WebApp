namespace Shared.Utilities.Results.Abstract
{
    //result ile birlikte veri tasima icin
    //out T: tek bir kategori de tasinabilir veya bir liste tasinabilir olur 
    public interface IDataResult<out T> : IResult
    {
        public T Data { get; }
        //ornek
        //new DataResult<category>(ResultStatus.Success,category);katmanlar arasi gonderim
        //new DataResult<IList<Category>>(ResultStatus.Success,categoryList)
    }
}
