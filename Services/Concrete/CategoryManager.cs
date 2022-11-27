using AutoMapper;
using Data.Concrete;
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
        private readonly UnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CategoryManager(UnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName)
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
            var category = _mapper.Map<Category>(categoryAddDto);
            if (category != null)
            {
                category.CreatedByName = createdByName;
                category.ModifiedByName = createdByName;
                await _unitofWork.Categories.AddAsync(category).ContinueWith(t => _unitofWork.SaveAsync());
                return new Result(ResultStatus.Success, $"Eklenen kategori \n {categoryAddDto.Name}");
            }
            return new Result(ResultStatus.Error, "Beklenmedik bir hata olustu");

        }

        public async Task<IResult> Delete(int categoryId)
        {
            var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId);
            if (category != null)
            {
                await _unitofWork.Categories.DeleteAsync(category).ContinueWith(t => _unitofWork.SaveAsync());
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
            return new DataResult<CategoryDto>(ResultStatus.Error, "Kategori bulunamadi", null);
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
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Kategori bulunamadi", null);
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
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Kategori bulunamadi", null);
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
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Boyle bir kategori bulunamadi", null);
        }

        public async Task<IResult> Remove(int categoryId, string modifiedByName)
        {
            var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                await _unitofWork.Categories.UpdateAsync(category).ContinueWith(t => _unitofWork.SaveAsync());
                return new Result(ResultStatus.Success, $"Kaldirilan kategori: \n{category.Name}");
            }
            return new Result(ResultStatus.Error, "Beklenmedik bir hata olustu, lutfen daha sonra tekrar deneyiniz");
        }

        public async Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            //var category = await _unitofWork.Categories.GetAsync(c => c.ID == categoryUpdateDto.Id);
            var category = _mapper.Map<Category>(categoryUpdateDto);
            if (category != null)
            {
                //category.Name = categoryUpdateDto.Name;
                //category.Note = categoryUpdateDto.Note;
                //category.IsActive = categoryUpdateDto.isActive;
                //category.ModifiedByName = modifiedByName;
                //category.ModifiedDate = DateTime.Now;
                //category.IsDeleted = categoryUpdateDto.isDeleted;
                //automapper daha az islemle yapar
                category.ModifiedByName = modifiedByName;
                await _unitofWork.Categories.UpdateAsync(category).ContinueWith(t => _unitofWork.SaveAsync());
                return new Result(ResultStatus.Success, $"Guncellenen kategori \n {categoryUpdateDto.Name}");
            }
            return new Result(ResultStatus.Error, "Kategori bulunamadi");
        }
    }
}
