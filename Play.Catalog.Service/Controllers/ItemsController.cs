using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository itemsRepository)
        {

            _itemsRepository = itemsRepository;
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

            await _itemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
                return NotFound();

            _itemsRepository.DeleteASync(existingItem);
            return NoContent();
        }

    }
}
