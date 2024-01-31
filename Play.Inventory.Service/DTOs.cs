namespace Play.Inventory.Service
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);

    //public record InventoryItemDto(Guid CatalogItemId, int Quantity, DateTimeOffset AcquiredDate)
    //{
    public record InventoryItemDto(Guid itemCatalogId, string catalogItemName, string catalogItemDescription,
        int itemQuantity, DateTime itemAcquireDate);

    public record CatalogItemDto(Guid id, string Name, string description);
    //}
}
