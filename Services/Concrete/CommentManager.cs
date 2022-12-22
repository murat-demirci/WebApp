using Data.Abstract;
using Services.Abstract;
using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;
using Shared.Utilities.Results.Concrete;

namespace Services.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly IUnitofWork _unitofWork;

        public CommentManager(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var commentCount = await _unitofWork.Comments.CountAsync();
            //makale sayisi alma
            if (commentCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmedik bir hata olustu", -1);
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var commentCount = await _unitofWork.Comments.CountAsync(c => !c.IsDeleted);
            //makale sayisi alma
            if (commentCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentCount);
            }
            return new DataResult<int>(ResultStatus.Error, "Beklenmedik bir hata olustu", -1);
        }
    }
}
