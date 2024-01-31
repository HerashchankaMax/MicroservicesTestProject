namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient _catalogClient;

        public CatalogClient(HttpClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemDTO()
        {
            return await _catalogClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
        }
    }
}
