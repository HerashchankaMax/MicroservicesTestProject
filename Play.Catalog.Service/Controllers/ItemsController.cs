using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Common.Interfaces;
using Play.Catalog.Contracts;


namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndPoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndPoint)
        {
            _itemsRepository = itemsRepository;
            _publishEndPoint = publishEndPoint;
        }
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            return (await _itemsRepository.GetAll()).AsItemsDtos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsItemDto();
        }


        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.description,
                Price = createItemDto.price,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            await _itemsRepository.CreateAsync(item);
            await _publishEndPoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.description;
            existingItem.Price = updateItemDto.price;

            await _publishEndPoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));
            await _itemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
                return NotFound();

            await _publishEndPoint.Publish(new CatalogItemDeleted(id));
            await _itemsRepository.DeleteASync(existingItem);
            return NoContent();
        }

    }
}
