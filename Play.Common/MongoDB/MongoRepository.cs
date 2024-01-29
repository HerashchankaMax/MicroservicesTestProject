using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common.Interfaces;

namespace Play.Common.MongoDB
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private IMongoCollection<T> _dbCollection;
        private FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;
        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
        {
            _dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAll() => await _dbCollection.Find(filterBuilder.Empty).ToListAsync();
        public async Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            var filter = filterBuilder.Eq(x => x.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var existingItemFilter = filterBuilder.Eq(existing => existing.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(existingItemFilter, entity);
        }

        public async Task DeleteASync(T entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var existingItemFilter = filterBuilder.Eq(existing => existing.Id, entity.Id);
            await _dbCollection.DeleteOneAsync(existingItemFilter);
        }

    }
}
