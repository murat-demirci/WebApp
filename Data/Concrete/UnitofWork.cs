using Data.Abstract;
using Data.Concrete.EntityFramework.Contexts;
using Data.Concrete.EntityFramework.Repositories;

namespace Data.Concrete
{
    public class UnitofWork : IUnitofWork
    {
        private readonly dContext _context;//kendi dbcontextimiz
        //articles,categories bizzden context bekledogi icin
        private efArticleRepository _articlerep;
        private efCategoryRepository _categoryrep;
        private efCommentRepository _commentrep;
        private efRoleRepository _rolerep;
        private efUserRepository _userrep;
        //interface newlenemediginden somut halleri eklenmeli
        public UnitofWork(dContext context)
        {
            _context = context;
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();//context de asenkron sekilde dispose edilir
            //islemi bellekten cikartir
        }
        public IArticleRepository Articles => _articlerep ?? new efArticleRepository(_context);//Iarticlerepository istendiginde somut halini doner
        //?? ilk deger null ise ikinci degeri don

        public ICategoryRepository Categories => _categoryrep ?? new efCategoryRepository(_context);

        public ICommentRepository Comments => _commentrep ?? new efCommentRepository(_context);

        public IRoleRepository Roles => _rolerep ?? new efRoleRepository(_context);

        public IUserRepository Users => _userrep ?? new efUserRepository(_context);
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();//int deger doner
        }
    }
}
