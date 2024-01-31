using Play.Common.Interfaces;

namespace Play.Inventory.Service
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogId { get; set; }
        public int Quantity { get; set; }
        public DateTime AcquireDate { get; set; }

    }
}
