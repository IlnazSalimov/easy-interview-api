using EasyInterview.API.Data;
using EasyInterview.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private EasyInterviewContext DbContext { get; set; }

        public Repository() { }

        public Repository(EasyInterviewContext context)
        {
            DbContext = context;
        }

        public async Task Delete(TEntity entity)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await DbContext.FindAsync<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public async Task<int> Save(TEntity entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity.Id;
        }
    }
}
