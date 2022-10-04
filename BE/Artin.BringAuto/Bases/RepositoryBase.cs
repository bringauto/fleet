using Artin.BringAuto.DAL;
using Artin.BringAuto.Extensions;
using Artin.BringAuto.Shared;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace BringAuto.Server.Bases
{
    public class RepositoryBase<TDal, TDto, TNew, TUpdate, TId> : IRepository<TDto, TNew, TUpdate, TId>
        where TDal : class, IId<TId>, new()
        where TDto : class, IId<TId>
        where TUpdate : class, IId<TId>
        where TId : struct
    {
        protected readonly BringAutoDbContext dbContext;
        protected readonly IMapper mapper;
        public RepositoryBase(BringAutoDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        private readonly SemaphoreSlim readLock = new SemaphoreSlim(1, 1);
        Dictionary<TId, TDto> scopeCache { get; } = new Dictionary<TId, TDto>();
        public async Task<TDto> GetByIdAsync(TId id)
        {
            await readLock.WaitAsync();
            try
            {
                TDto result;
                if (!scopeCache.TryGetValue(id, out result))
                {
                    scopeCache[id] = result = await dbContext.Set<TDal>()
                                                             .SoftDeleteFilter()
                                                             .ProjectTo<TDto>(mapper.ConfigurationProvider)
                                                             .FirstOrDefaultAsync(x => x.Id.Equals(id));
                }
                return result;
            }
            finally
            {
                readLock.Release();
            }
        }

        public async Task<TDto> UpdateAsync(TUpdate dto)
        {
            TDal dal = AddIncludesForUpdate(dbContext.Set<TDal>().SoftDeleteFilter()).FirstOrDefault(x => x.Id.Equals(dto.Id));
            BeforeUpdateMap(dal);
            dal = mapper.Map(dto, dal);
            await BeforeUpdate(dal);
            dbContext.Set<TDal>().Update(dal);
            await dbContext.SaveChangesAsync();
            await AfterUpdate(dal);

            return await GetByIdAsync(dto.Id);
        }

        protected virtual IQueryable<TDal> AddIncludesForUpdate(IQueryable<TDal> source)
            => source;

        protected virtual void BeforeUpdateMap(TDal dal)
        {
        }

        public async Task<TDto> AddAsync(TNew dto)
        {
            TDal dal = mapper.Map<TDal>(dto);
            dbContext.Set<TDal>().Add(dal);
            await BeforeAdd(dal);
            await dbContext.SaveChangesAsync();
            await AfterAdd(dal);
            await dbContext.SaveChangesAsync();

            return await GetByIdAsync(dal.Id);
        }

        public async Task<TDto> DeleteAsync(TId id)
        {
            TDto ent = await GetByIdAsync(id);
            BeforeDelete(id);
            var entity = await dbContext.Set<TDal>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (entity is ISoftDelete softDelete)
            {
                softDelete.Deleted = true;
            }
            else
            {
                dbContext.Set<TDal>().Remove(entity);
            }
            return (await dbContext.SaveChangesAsync() > 0) ? ent : null;
        }

        protected virtual void BeforeDelete(TId id)
        {
        }

        public IQueryable<TDto> Load()
        {
            return dbContext.Set<TDal>().SoftDeleteFilter().ProjectTo<TDto>(mapper.ConfigurationProvider);
        }

        protected virtual Task BeforeAdd(TDal entity) => Task.CompletedTask;
        protected virtual Task AfterAdd(TDal entity) => Task.CompletedTask;

        protected virtual Task BeforeUpdate(TDal entity) => Task.CompletedTask;
        protected virtual Task AfterUpdate(TDal entity) => Task.CompletedTask;
    }
}
