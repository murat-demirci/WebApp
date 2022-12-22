using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetlAllByNoneDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetlAllByNoneDeletedAnActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdName);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedName);
        Task<IResult> DeleteAsync(int articleId);
        Task<IResult> RemoveAsync(int articleId, string modifiedName);
        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
