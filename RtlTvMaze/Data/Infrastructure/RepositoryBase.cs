using System.Linq.Expressions;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RtlTvMaze.Data.Infrastructure.Interface;
using RtlTvMaze.Domain;

namespace RtlTvMaze.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T, string> where T : BaseEntity, new()
    {
        protected readonly IMongoCollection<T> Collection;
        private readonly MongoDbSettings settings;

        protected RepositoryBase(IOptions<MongoDbSettings> options)
        {
            this.settings = options.Value;
            var client = new MongoClient(this.settings.ConnectionString);
            var db = client.GetDatabase(this.settings.Database);
            this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? Collection.AsQueryable()
                : Collection.AsQueryable().Where(predicate);
        }

        public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).ToListAsync();
        }


        public virtual Task<List<T>> GetListWithPaginationAsync(Expression<Func<T, bool>> predicate, int page, int size)
        {
            return Collection.Find(predicate).Skip(page == null ? 0: (page-1) * size).Limit(size).ToListAsync();
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await Collection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            return await Collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            return await Collection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await Collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<long> GetListCountAsync(Expression<Func<T, bool>> predicate)
        {
            return Collection.Find(predicate).Count();
        }
    }
}
