using Microsoft.AspNetCore.Mvc;
using Play.Common.Interfaces;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Extensions;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;
        private readonly CatalogClient _catalogClient;

        public InventoryController(IRepository<InventoryItem> inventoryRepository, CatalogClient catalogClient)
        {
            _inventoryRepository = inventoryRepository;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await _catalogClient.GetCatalogItemDTO();
            var t = await _inventoryRepository.GetAll();
            var items = (await _inventoryRepository.GetAll(x => x.UserId == userId))
                .Select(item =>
                {
                    var catalogItem = catalogItems.Single(x => x.id == item.CatalogId);
                    return item.AsDto(catalogItem.Name, catalogItem.description);
                });

            return Ok(items);
        }
        [HttpPost]
        public async Task<ActionResult> Post(GrantItemsDto inventoryItemDto)
        {
            var requestedItem = await _inventoryRepository.GetAsync(x =>
                x.CatalogId == inventoryItemDto.CatalogItemId &&
                x.UserId == inventoryItemDto.UserId);

            if (requestedItem == null)
            {
                requestedItem = new InventoryItem
                {
                    UserId = inventoryItemDto.UserId,
                    CatalogId = inventoryItemDto.CatalogItemId,
                    Quantity = inventoryItemDto.Quantity,
                    AcquireDate = DateTime.UtcNow
                };
                await _inventoryRepository.CreateAsync(requestedItem);
            }
            else
            {
                requestedItem.Quantity += inventoryItemDto.Quantity;
                await _inventoryRepository.UpdateAsync(requestedItem);
            }
            return Ok();
        }
    }
}
