using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amout of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get() => items;

        [HttpGet("{id}")]
        public ItemDto GetById(Guid id)
        {
            return items.First(x => x.id == id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.FirstOrDefault(x => x.id == id);
            if (existingItem == null)
                return NotFound();

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                description = updateItemDto.description,
                price = updateItemDto.price,
            };

            var index = items.FindIndex(x => x.id == id);
            items[index] = updatedItem;
            return NoContent();
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.description, createItemDto.price,
                DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(x => x.id == id);
            if (index < 0)
                return NotFound();
            items.RemoveAt(index);
            return NoContent();
        }

    }
}
