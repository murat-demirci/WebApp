using Shared.Utilities.Results.ComplexTypes;

namespace Shared.Entites.Abstract
{
    public abstract class DtoGetBase
    {
        public virtual ResultStatus resultStatus { get; set; }
        public virtual string Message { get; set; }

        /* ============== Sayfalama İşlemleri ============== */
        public int CurrentPage { get; set; } = 1;

        //sayfalarda kaç tane makale gösterileceğini tutar
        public int PageSize { get; set; } = 5;
        //kaç tane makale olduğunu tutar
        public int TotalCount { get; set; }
        //kaç sayfa olacağını tutar, Ceiling eşit olan veya yukarıdaki integer'a yuvarlar (7 / 2'yi 4'e yuvarlar)
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        //önceki sayfa butonunu gösterebilmek için CurrentPage'in 1'den büyük olması lazım. Bunun kontrolünü sağlar
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
    }
}
