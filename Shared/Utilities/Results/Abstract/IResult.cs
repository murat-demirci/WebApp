using Shared.Utilities.Results.ComplexTypes;

namespace Shared.Utilities.Results.Abstract
{
    //utilities(araclar icinde) islem basarili mi bsarisiz mi sonucu doner (mvc katmanina)
    //resultun durumu paylasilmali
    public interface IResult
    {
        public ResultStatus ResultStatus { get; }//ResultStatus.error gibi kullanilacak  
        public string Message { get; }
        public Exception Exception { get; }
    }
}
