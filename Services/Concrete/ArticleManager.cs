using AutoMapper;
using Data.Abstract;
using Entities.Concrete;
using Entities.Dtos.Articles;
using Services.Abstract;
using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;
using Shared.Utilities.Results.Concrete;

namespace Services.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitofWork unitofWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitofWork = unitofWork;
        }


        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdName)
        {
            //map isleminden sonra hangi tip donecegi verilir (Map<>)
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdName;
            article.ModifiedByName = createdName;
            article.UserId = 1;
            await _unitofWork.Articles.AddAsync(article).ContinueWith(t => _unitofWork.SaveAsync());
            return new Result(ResultStatus.Success, $"{article.Title} baslikli makale eklenmistir");
        }

        public async Task<IResult> Delete(int articleId)
        {
            var result = await _unitofWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await _unitofWork.Articles.GetAsync(a => a.ID == articleId);
                await _unitofWork.Articles.DeleteAsync(article).ContinueWith(t => _unitofWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{article.Title} baslikli makale silinmistir");
            }
            return new Result(ResultStatus.Error, $"Boyle bir makale bulunamadi");
        }

        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            var article = await _unitofWork.Articles.GetAsync(a => a.ID == articleId, a => a.User, a => a.Category);
            if (article != null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, "Boyle bir makale bulunamadi", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Makaleler bulunamadi", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var result = await _unitofWork.Categories.AnyAsync(c => c.ID == categoryId);
            if (result)
            {
                var articles = await _unitofWork.Articles.GetAllAsync(a => a.CategoryId == categoryId, a => a.User);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        resultStatus = ResultStatus.Success,
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, "Bu kategoriye ait makale bulunamadi", null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Kategori bulunamadi", null);

        }

        public async Task<IDataResult<ArticleListDto>> GetlAllByNoneDeleted()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(a => !a.IsDeleted, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Silinmis Makale veya Makaleler bulunamadi", null);
        }

        public async Task<IDataResult<ArticleListDto>> GetlAllByNoneDeletedAnActive()
        {
            var articles = await _unitofWork.Articles.GetAllAsync(a => !a.IsDeleted && !a.IsDeleted, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, "Silinmis ve aktif Makale veya Makaleler bulunamadi", null);
        }

        public async Task<IResult> Remove(int articleId, string modifiedName)
        {
            var result = await _unitofWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await _unitofWork.Articles.GetAsync(a => a.ID == articleId);
                article.ModifiedByName = modifiedName;
                article.IsDeleted = true;
                article.ModifiedDate = DateTime.Now;
                await _unitofWork.Articles.UpdateAsync(article).ContinueWith(t => _unitofWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{article.Title} baslikli makale kaldirildi");
            }
            return new Result(ResultStatus.Error, $"Boyle bir makale bulunamadi");
        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedName)
        {
            //map isleminden sonra hangi tip donecegi verilir (Map<>)
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifiedName;
            article.UserId = 1;
            await _unitofWork.Articles.AddAsync(article).ContinueWith(t => _unitofWork.SaveAsync());
            return new Result(ResultStatus.Success, $"{article.Title} baslikli makale guncellenmistir");
        }
    }
}
