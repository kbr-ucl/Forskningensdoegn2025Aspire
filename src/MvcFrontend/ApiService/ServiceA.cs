namespace MvcFrontend.ApiService;

public class ServiceA
{
    private readonly HttpClient _api;

    public ServiceA(HttpClient httpClient)
    {
        _api = httpClient;
    }

    public async Task<IEnumerable<ServiceAEntityDto>> GetServiceAEntities()
    {
        var response = await _api.GetAsync("ServiceAEntities");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ServiceAEntityDto>>();
        return result ?? [];
    }

    public async Task<ServiceAEntityDto> GetServiceAEntity(int id)
    {
        var response = await _api.GetAsync($"ServiceAEntities/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ServiceAEntityDto>();
        return result ?? throw new Exception("Entity not found");
    }

    public async Task<ServiceAEntityDto> CreateServiceAEntity(ServiceAEntityDto dto)
    {
        var response = await _api.PostAsJsonAsync("ServiceAEntities", dto);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ServiceAEntityDto>();
        return result ?? throw new Exception("Failed to create entity");
    }

    public async Task UpdateServiceAEntity(int id, ServiceAEntityDto dto)
    {
        var response = await _api.PutAsJsonAsync($"ServiceAEntities/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteServiceAEntity(int id)
    {
        var response = await _api.DeleteAsync($"ServiceAEntities/{id}");
        response.EnsureSuccessStatusCode();
    }
}

// DTO for ServiceAEntity
public record ServiceAEntityDto(int Id, string Name, string Description);