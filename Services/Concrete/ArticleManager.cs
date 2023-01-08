using AutoMapper;
using Data.Abstract;
using Data.Concrete;
using Entities.ComplexTypes;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Abstract;
using Services.Utilities;
using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;
using Shared.Utilities.Results.Concrete;
using System.Linq.Expressions;
using static Services.Utilities.Messages;
using Article = Entities.Concrete.Article;

namespace Services.Concrete
{
    public class ArticleManager : ManagerBase, IArticleService
    {
        private readonly UserManager<User> _userManager;
        public ArticleManager(IUnitofWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId, a => a.User, a => a.Category);
            if (article != null)
            {
                article.Comments = await UnitOfWork.Comments.GetAllAsync(c => c.ArticleId == articleId && !c.IsDeleted && c.IsActive);
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    resultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId);
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
            }
            else
            {
                return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => !a.IsDeleted, ar => ar.User, ar => ar.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var articles =
                await UnitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, ar => ar.User,
                    ar => ar.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(c => c.ID == categoryId);
            if (result)
            {
                var articles = await UnitOfWork.Articles.GetAllAsync(
                    a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, ar => ar.User, ar => ar.Category);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        resultStatus = ResultStatus.Success
                    });
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), null);

        }

        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = Mapper.Map<Entities.Concrete.Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId;
            await UnitOfWork.Articles.AddAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.Add(article.Title));
        }

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await UnitOfWork.Articles.GetAsync(a => a.ID == articleUpdateDto.Id);
            var article = Mapper.Map<ArticleUpdateDto, Entities.Concrete.Article>(articleUpdateDto, oldArticle);
            article.ModifiedByName = modifiedByName;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId);
                article.IsDeleted = true;
                article.IsActive = false;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }
            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
        }

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId);
                await UnitOfWork.Articles.DeleteAsync(article);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }
            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync();
            if (articlesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync(a => !a.IsDeleted);
            if (articlesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
        }


        public async Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync()
        {
            var articles =
                await UnitOfWork.Articles.GetAllAsync(a => a.IsDeleted, ar => ar.User,
                    ar => ar.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    resultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), null);
        }


        public async Task<IDataResult<ArticleDto>> UndoDeleteAsync(int articleId, string modifiedByName)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.ID == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId);
                article.IsDeleted = false;
                article.IsActive = true;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();
                return new DataResult<ArticleDto>(ResultStatus.Success, Messages.Article.UndoDelete(article.Title), new ArticleDto
                {
                    Article = article,
                    resultStatus = ResultStatus.Success,
                    Message = Messages.Article.UndoDelete(article.Title)
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(false), new ArticleDto
            {
                Article = null,
                resultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(false)
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);
            var sortedArticles = isAscending 
                ? articles.OrderBy(a => a.ArticleView) 
                : articles.OrderByDescending(a => a.ArticleView);
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList(),
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = categoryId == null
                ? await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User)
                : await UnitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId.Value && a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);
            //skip sql'deki offset gibi davranır, yani belirli sayıdaki makaleyi geçmemizi sağlar 
            //örneğin her sayfada 5 makale olsun ve 2. sayfada olduğumuz varsayılsın. 2. sayfadaki makaleleri göstermek için
            //5 tane makaleyi geçmemiz gerek. (2 - 1) * 5 = 5 ile bu işlem gerçekleştirilir
            //take ile'de alınacak makale sayısı belirlenir
            var sortedArticles = isAscending
                ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId= categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
            });
        }

        public async Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);
                var sortedArticles = isAscending
                    ? articles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : articles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = sortedArticles,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = articles.Count
                });
            }
            var searchedArticles = await UnitOfWork.Articles.SearchAsync(new List<Expression<Func<Article, bool>>>
            {
                (a) => a.Title.Contains(keyword),
                (a) => a.Category.Name.Contains(keyword),
                (a) => a.SeoDescription.Contains(keyword),
                (a) => a.SeoTags.Contains(keyword),

            }, a => a.Category, a => a.User);
            var searchedAndSortedArticles = isAscending
                    ? searchedArticles.OrderBy(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : searchedArticles.OrderByDescending(a => a.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = searchedAndSortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedArticles.Count
            });
        }

        public async Task<IResult> IncreaseViewCountAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.ID == articleId);
            if(article == null)
            {
                return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
            }
            article.ArticleView += 1;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.IncreaseViewCount(article.Title));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var anyUser = await _userManager.Users.AnyAsync(u => u.Id == userId);
            if (!anyUser)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, "Kullanıcı bulanamadı", null);
            }
            var userArticles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.UserId == userId);
            List<Article> sortedArticles = new List<Article>();
            switch (filterBy)
            {
                case FilterBy.Category:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.CreatedDate).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CreatedDate).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.ArticleView).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ArticleView).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.ArticleComment).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ArticleComment).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.Date:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderBy(a => a.CreatedDate).ToList()
                                : userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderByDescending(a => a.CreatedDate).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderBy(a => a.ArticleView).ToList()
                                : userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderByDescending(a => a.ArticleView).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderBy(a => a.ArticleComment).ToList()
                                : userArticles.Where(a => a.CreatedDate >= startAt && a.CreatedDate <= endAt).Take(takeSize).OrderByDescending(a => a.ArticleComment).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.ViewCount:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderBy(a => a.CreatedDate).ToList()
                                : userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderByDescending(a => a.CreatedDate).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderBy(a => a.ArticleView).ToList()
                                : userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderByDescending(a => a.ArticleView).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderBy(a => a.ArticleComment).ToList()
                                : userArticles.Where(a => a.ArticleView >= minViewCount && a.ArticleView <= maxViewCount).Take(takeSize).OrderByDescending(a => a.ArticleComment).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.CommentCount:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleComment>= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderBy(a => a.CreatedDate).ToList()
                                : userArticles.Where(a => a.ArticleComment >= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.CreatedDate).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleComment >= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderBy(a => a.ArticleView).ToList()
                                : userArticles.Where(a => a.ArticleComment >= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.ArticleView).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ArticleComment >= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderBy(a => a.ArticleComment).ToList()
                                : userArticles.Where(a => a.ArticleComment >= minCommentCount && a.ArticleComment <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.ArticleComment).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles
            });
        }
    }
}
