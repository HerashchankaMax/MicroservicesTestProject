namespace Play.Inventory.Service.Extensions
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string catalogItemName, string catalogItemDescription)
        {
            return new InventoryItemDto(item.CatalogId, catalogItemName, catalogItemDescription, item.Quantity, item.AcquireDate);
        }

    }
}
