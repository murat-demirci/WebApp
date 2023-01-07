using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        Task<IDataResult<ArticleDto>> UndoDeleteAsync(int articleId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int articleId);
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();
        //take size nullable. sayı verilmezse tüm makaleler gelir, verilirse verilen sayı kadar makale gelir
        Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        
        //current page gelmezse 1 olarak atanır
        //isAscending sıralama türünü tutar
        Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
    }
}
