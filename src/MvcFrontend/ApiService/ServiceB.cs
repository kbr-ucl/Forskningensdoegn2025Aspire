namespace MvcFrontend.ApiService
{
    public class ServiceB
    {
        private readonly HttpClient _api;

        public ServiceB(HttpClient httpClient)
        {
            _api = httpClient;
        }

        public async Task<IEnumerable<ServiceBEntityDto>> GetServiceBEntities()
        {
            var response = await _api.GetAsync("ServiceBEntities");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ServiceBEntityDto>>();
            return result ?? [];
        }

        public async Task<ServiceBEntityDto> GetServiceBEntity(int id)
        {
            var response = await _api.GetAsync($"ServiceBEntities/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ServiceBEntityDto>();
            return result ?? throw new Exception("Entity not found");
        }

        public async Task<ServiceBEntityDto> CreateServiceBEntity(ServiceBEntityDto dto)
        {
            var response = await _api.PostAsJsonAsync("ServiceBEntities", dto);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ServiceBEntityDto>();
            return result ?? throw new Exception("Failed to create entity");
        }

        public async Task UpdateServiceBEntity(int id, ServiceBEntityDto dto)
        {
            var response = await _api.PutAsJsonAsync($"ServiceBEntities/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteServiceBEntity(int id)
        {
            var response = await _api.DeleteAsync($"ServiceBEntities/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

    // DTO for ServiceBEntity
    public record ServiceBEntityDto(int Id, string Name, string Description);
}
