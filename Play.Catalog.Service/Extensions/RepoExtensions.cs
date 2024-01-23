using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Extensions
{
    public static class RepoExtensions
    {
        public static Item AsItem(this ItemDto item)
        {
            return new Item
            {
                Id = item.id,
                Name = item.Name,
                Description = item.description,
                Price = item.price,
                CreatedDate = item.createdDate,
            };
        }
        public static ItemDto AsItemDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }

        public static List<ItemDto> AsItemsDtos(this IEnumerable<Item> items)
        {
            return items.Select(x => x.AsItemDto()).ToList();
        }

    }
}
