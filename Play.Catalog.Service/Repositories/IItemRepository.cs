using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public interface IItemsRepository
{
    Task<IReadOnlyCollection<Item>> GetAll();
    Task<Item> GetAsync(Guid id);
    Task CreateAsync(Item entity);
    Task UpdateAsync(Item entity);
    Task DeleteASync(Item entity);
}
