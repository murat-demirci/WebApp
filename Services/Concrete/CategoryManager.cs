using AutoMapper;
using Data.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Categories;
using Services.Abstract;
using Shared.Utilities.Results.Abstract;
using Shared.Utilities.Results.ComplexTypes;
using Shared.Utilities.Results.Concrete;

namespace Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
            //burda el ile yapilacak diger managerlarda outomapper ile yapilacak
            //await _unitofWork.Categories.AddAsync(new Category
            //{
            //    Name = categoryAddDto.Name,
            //    Note = categoryAddDto.Note,
            //    IsActive = categoryAddDto.isActive,
            //    CreatedByName = createdByName,
            //    ModifiedByName = createdByName,
            //    CreatedDate = DateTime.Now,//bu entity de tanimli olmasa da olur (add icin)
            //    ModifiedDate = DateTime.Now,//bu entity de tanimli olmasa da olur (add icin)
            //    IsDeleted = false//bu entity de tanimli olmasa da olur (add icin)
            //}).ContinueWith(t => _unitofWork.SaveAsync());//ustteki islemi bitirip cok hizli bir sekilde save yapar
            //yani veritabanina eklenmeden ekrana gosterir
            //await _unitofWork.SaveAsync(); daha prof,daha hizli

            //entityframework threadsafe yap(saveasync() i await _unitofwork.saveasync() yap
            //, continue cikar calismazsa)
            //(saveasync() calisirken controlleri a geri donuldugunde farkli bir istekte bulunuldugunda
            //ikinci kez dbcontext kullanildiiginda hata aliniyor)
            var category = _mapper.Map<Category>(categoryAddDto);

            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory = await _unitofWork.Categories.AddAsync(category);
            await _unitofWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, $"Eklenen kategori \n {categoryAddDto.Name}", new CategoryDto
            {
                Category = addedCategory,
                resultStatus = ResultStatus.Success,
                Message = $"Eklenen kategori \n {categoryAddDto.Name}"
            });



        }

        public async Task<IResult> Delete(int categoryId)
        {
            var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId);
            if (category != null)
            {
                await _unitofWork.Categories.DeleteAsync(category);
                await _unitofWork.SaveAsync();
                return new Result(ResultStatus.Success, $"Silinen kategori: \n{category.Name}");
            }
            return new Result(ResultStatus.Error, "Beklenmedik bir hata olustu, lutfen daha sonra tekrar deneyiniz");
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {
            var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId, c => c.Articles);
            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = category,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Kategori bulunamadi", new CategoryDto
            {
                Category = null,
                resultStatus = ResultStatus.Error,
                Message = "Kategori bulunamadi"
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Kategori bulunamadi", new CategoryListDto
            {
                Categories = null,
                resultStatus = ResultStatus.Error,
                Message = "Kategori bulunamadi"
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Kategori bulunamadi", new CategoryListDto
            {
                Categories = null,
                resultStatus = ResultStatus.Error,
                Message = "Kategori bulunamadi"
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories = await _unitofWork.Categories.GetAllAsync(c => c.IsActive && !c.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    resultStatus = ResultStatus.Success,
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Boyle bir kategori bulunamadi", new CategoryListDto
            {
                Categories = null,
                resultStatus = ResultStatus.Error,
                Message = "Kategori bulunamadi"
            });
        }

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
        {
            var result = await _unitofWork.Categories.AnyAsync(c => c.ID == categoryId);
            if (result)
            {
                var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, "Boyle bir kategori bulunamadi", null);
        }

        public async Task<IDataResult<CategoryDto>> Remove(int categoryId, string modifiedByName)
        {
            var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await _unitofWork.Categories.UpdateAsync(category);
                await _unitofWork.SaveAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success, $"Kaldirilan kategori \n {deletedCategory.Name}", new CategoryDto
                {
                    Category = deletedCategory,
                    resultStatus = ResultStatus.Success,
                    Message = $"Kaldirilan kategori \n {deletedCategory.Name}"
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Boyle bir kategori bulunamadi", new CategoryDto
            {
                Category = null,
                resultStatus = ResultStatus.Error,
                Message = "Boyle bir kategori bulunamadi"
            });
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await _unitofWork.Categories.GetAsync(c => c.ID == categoryUpdateDto.Id);
            //var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
            //kategori ile ilgili updatedto da bulunmayayn olsuturan olusturulma tarihi gibi degerleride almis olacagiz

            //category.Name = categoryUpdateDto.Name;
            //category.Note = categoryUpdateDto.Note;
            //category.IsActive = categoryUpdateDto.isActive;
            //category.ModifiedByName = modifiedByName;
            //category.ModifiedDate = DateTime.Now;
            //category.IsDeleted = categoryUpdateDto.isDeleted;
            //automapper daha az islemle yapar
            category.ModifiedByName = modifiedByName;
            var updatedCategory = await _unitofWork.Categories.UpdateAsync(category);
            await _unitofWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, $"Guncellenen kategori \n {categoryUpdateDto.Name}", new CategoryDto
            {
                Category = updatedCategory,
                resultStatus = ResultStatus.Success,
                Message = $"Guncellenen kategori \n {categoryUpdateDto.Name}"
            });
        }
    }
}
