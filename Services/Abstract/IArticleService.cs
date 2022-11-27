using Entities.Dtos.Articles;
using Shared.Utilities.Results.Abstract;

namespace Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> Get(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetlAllByNoneDeleted();
        Task<IDataResult<ArticleListDto>> GetlAllByNoneDeletedAnActive();
        Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedName);
        Task<IResult> Delete(int articleId);
        Task<IResult> Remove(int articleId, string modifiedName);
    }
}
