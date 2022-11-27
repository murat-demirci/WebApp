using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;

namespace Shared.Utilities.Results.Concrete
{
    public class Result : IResult
    {
        public Result(ResultStatus ResultStatus)
        {
            ResultStatus = ResultStatus;
            //durumu ogrenmek icin
        }
        public Result(ResultStatus ResultStatus, string message)
        {
            ResultStatus = ResultStatus;
            Message = message;
            //durumu ogrenmek icin
        }
        public Result(ResultStatus ResultStatus, string message, Exception exception)
        {
            ResultStatus = ResultStatus;
            Message = message;
            Exception = exception;
            //durumu ogrenmek icin
        }
        public ResultStatus ResultStatus { get; }

        public string Message { get; }

        public Exception Exception { get; }
        //kullanim ornegi
        //new Result(ResultStatus.Error,"islem basarisiz"(veya exception.message),exception)
    }
}
