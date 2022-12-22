using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Services.Abstract
{
    //veriyi servis edilen katman
    //backend en gelen verinin frontend e gitmeden once islendigi kisim
    //asenkron yapi oldugundan task olarak eklenecek
    public interface ICategoryService
    {
        ///<summary>
        ///tek bir categorydto almak icin kullanilir
        /// </summary> 
        /// <param name="categoryId">0 dan buyuk int deger</param>
        /// <returns>Asenkron operasyon ile DataResult tipinde geri doner</returns>
        Task<IDataResult<CategoryDto>> GetAsync(int categoryId);
        ///<summary>
        ///Verilen id degerine ait tek bir categoryUpddateDto almak icin kullanilir
        /// </summary> 
        /// <param name="categoryId">0 dan buyuk int deger</param>
        /// <returns>Asenkron operasyon ile DataResult tipinde geri doner</returns>
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);
        //verileri tasimak icin olusturulan dataresult ve bu dataresult category tipinde
        //categoryid ver categoryi bulsun
        Task<IDataResult<CategoryListDto>> GetAllAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync();
        //aktif olanlari getirme islemi yap(optional)

        //artik veri ekleme guncellemede bir dto donulecek
        Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName);
        //dto veri transfer objesi (viewmodal)
        //frontend alaninda ihtiyacimiz olan alanlari barindirir
        //kategori eklerken adini ve aciklamasini istiyoruz modifiedbyname gibi kisimlari gostermiyor
        //yani kullanicidan sadece dtoya ait kisimlar isteniyor
        //dtolar entity katmaninda
        Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
        Task<IDataResult<CategoryDto>> RemoveAsync(int categoryId, string modifiedByName);//gecici silme
        //remove islemini idataresult yap. 
        Task<IResult> DeleteAsync(int categoryId);//tam silme
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
