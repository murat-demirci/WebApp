﻿using Entities.ComplexTypes;
using Entities.Dtos;
using Mvc.Helpers.Abstract;
using Shared.Utilities.Extensions;
using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;
using Shared.Utilities.Results.Concrete;

namespace Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        //wwroot dosya yolunu dinakilestirmek icin
        //farkli isletim sistemlerinde dosya yolu ayni kalir
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";
        private const string userImagesFolder = "UserImages";
        private const string postImagesFolder = "PostImages";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
            //wwwrootun dosya yolunu dinamik olarak verir
        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            //guncelleme isleminde eski resmi silmek icin kullanilacak
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", pictureName);
            if (File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                //dosyanin bilgilerini alir
                //dosya silindikten sonra bilgilere erisilemiyor (file not found)
                //silme isleminden once newlenme yapilmali
                var imageDeletedDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length,
                };
                File.Delete(fileToDelete);
                return new DataResult<ImageDeletedDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error, $"Boyle bir resim bulunamadi", null);
            }
        }

        public async Task<IDataResult<UploadedImageDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null)
        {
            //folderName null gelirse pictureType'a bakılarak folderName'e atama yapılır
            folderName ??= pictureType == PictureType.User ? userImagesFolder : postImagesFolder;

            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                //klasor var mi yok mu kontrolu
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");
            }
            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            //resim dosyasinin sonundaki eklentiyi almaz sadece dosya adi gelir\ ornek amacli yazildi
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            //dosya sonundaki format alinir
            string newFileName = $"{ImageExtensions.CreateGuid()}_{name}{fileExtension}";
            //dosya adi olusturulud
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName);
            //foldername kullanici resimlerini kullanici klasorunde saklamamizi saglayacak
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            //resim img klasorune aktarilir
            string message = pictureType == PictureType.User
                ? $"{name} adli kullanicinin resmi basariyla yuklenmistir"
                : $"{name} adli makalenin resmi basariyla yuklenmistir";
            return new DataResult<UploadedImageDto>
                (ResultStatus.Success, message, new UploadedImageDto
                {
                    FullName = $"{folderName}/{newFileName}",
                    OldName = oldFileName,
                    Extension = fileExtension,
                    FolderName = folderName,
                    Path = path,
                    Size = pictureFile.Length
                });
            }
    }
}
