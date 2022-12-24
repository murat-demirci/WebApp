using Entities.ComplexTypes;
using Entities.Dtos;
using Shared.Utilities.Results.Abstract;

namespace Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<UploadedImageDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType ,string folderName = null);
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
