using Microsoft.EntityFrameworkCore;
using Shared.Data.Abstract;
using Shared.Entites.Abstract;
using System.Data;
using System.Linq.Expressions;


namespace Shared.Data.Concrete.Dapper
{
    //shared nuget paket dapper(entityframework kullanildi aslinda) ekle
    //3. parti yazilim oldugunden klasorleme ayri yapildi
    public class efEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        //generic olmali, IEntityRepository de kosul oldugundan buraya da eklenmesi lazim
    {
        private readonly DbContext _context;//ctor olmali,entitiyframework
        public efEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;//degisim oldugundan entity geri donulur (ajax islemleri icin)
            //set entity ile tentitye abone olunur(article,comment,user olabilir)
            //addasync(user,comment,category olabilir)
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().CountAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
            //task islemi olusturuldu
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includeProperties.Any())
            {
                foreach (var include in includeProperties)
                {
                    query = query.Include(include);
                    //includeProperties bos degilse icindeki degerleride query e ekler
                }
            }
            return await query.ToListAsync();//list olarak doner
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includeProperties.Any())
            {
                foreach (var include in includeProperties)
                {
                    query = query.Include(include);
                    //includeProperties bos degilse icindeki degerleride query e ekler
                }
            }
            return await query.SingleOrDefaultAsync();//tek nesne olarak doner
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
            return entity;
        }
    }
}
