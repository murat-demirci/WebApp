using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadedImageDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "UserImages");
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
