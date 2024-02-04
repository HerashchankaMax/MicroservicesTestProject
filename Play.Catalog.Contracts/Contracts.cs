using System;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid CatalogId, string Name, string Description);
    public record CatalogItemUpdated(Guid CatalogId, string Name, string Description);
    public record CatalogItemDeleted(Guid CatalogId);
}
