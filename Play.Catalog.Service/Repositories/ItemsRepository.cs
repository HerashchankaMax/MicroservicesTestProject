using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class ItemsRepository : IItemsRepository
    {
        private IMongoCollection<Item> _dbCollection;
        private FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        public ItemsRepository(IMongoDatabase mongoDatabase)
        {
            _dbCollection = mongoDatabase.GetCollection<Item>("items");
        }

        public async Task<IReadOnlyCollection<Item>> GetAll() => await _dbCollection.Find(FilterDefinition<Item>.Empty).ToListAsync();

        public async Task<Item> GetAsync(Guid id)
        {
            var filter = filterBuilder.Eq(x => x.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var existingItemFilter = filterBuilder.Eq(existing => existing.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(existingItemFilter, entity);
        }

        public async Task DeleteASync(Item entity)
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
