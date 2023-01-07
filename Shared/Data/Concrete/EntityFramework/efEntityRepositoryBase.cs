﻿using LinqKit;
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
        protected readonly DbContext _context;//ctor olmali,entitiyframework
        //protected olmali, cunku tureyen diger siniflarda da dbcontexte erismek icin
        //ornek icin Icategoryrepository efcategory
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

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await (predicate == null ? _context.Set<TEntity>().CountAsync()
                : _context.Set<TEntity>().CountAsync(predicate));
            //null gelirse tum degerleri don null gelmezse filtreleme yap ve don
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
            query = query.Where(predicate);

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

        public async Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            //predicates listesi boş olabilir, bunu kontrol ederiz
            //predicates'in içerdikleri query'e eklenir
            if (predicates.Any())
            {
                var predicateChain = PredicateBuilder.New<TEntity>();
                foreach (var predicate in predicates)
                {
                    //Where işleminde predicate'ler AND operatörü ile birbirine bağlanır
                    //query = query.Where(predicate);
                    //OR ile bağlamak için LinqKit.Microsoft.EntityFramework paketi yüklenir
                    predicateChain.Or(predicate);
                }
                //oluşturulmuş olan OR'lu predicateChain query'e eklenir
                query = query.Where(predicateChain);
            }
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }
    }
}
