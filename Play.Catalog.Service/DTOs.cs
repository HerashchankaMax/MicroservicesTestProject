using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service
{
    public record ItemDto(Guid id, string Name, string description, decimal price, DateTimeOffset createdDate);
    public record CreateItemDto([Required] string Name, string description, [Range(0, 1000)] decimal price);
    public record UpdateItemDto([Required] string Name, string description, [Range(0, 1000)] decimal price);
}
