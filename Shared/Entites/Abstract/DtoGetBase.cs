using Shared.Utilities.Results.ComplexTypes;

namespace Shared.Entites.Abstract
{
    public abstract class DtoGetBase
    {
        public virtual ResultStatus resultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
