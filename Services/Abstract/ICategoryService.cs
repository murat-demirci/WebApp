using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Services.Abstract
{
    //veriyi servis edilen katman
    //backend en gelen verinin frontend e gitmeden once islendigi kisim
    //asenkron yapi oldugundan task olarak eklenecek
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDto>> Get(int categoryId);
        Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId);
        //verileri tasimak icin olusturulan dataresult ve bu dataresult category tipinde
        //categoryid ver categoryi bulsun
        Task<IDataResult<CategoryListDto>> GetAll();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeleted();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive();
        //aktif olanlari getirme islemi yap(optional)

        //artik veri ekleme guncellemede bir dto donulecek
        Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName);
        //dto veri transfer objesi (viewmodal)
        //frontend alaninda ihtiyacimiz olan alanlari barindirir
        //kategori eklerken adini ve aciklamasini istiyoruz modifiedbyname gibi kisimlari gostermiyor
        //yani kullanicidan sadece dtoya ait kisimlar isteniyor
        //dtolar entity katmaninda
        Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
        Task<IDataResult<CategoryDto>> Remove(int categoryId, string modifiedByName);//gecici silme
        //remove islemini idataresult yap. 
        Task<IResult> Delete(int categoryId);//tam silme
        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountByIsDeleted();
    }
}
