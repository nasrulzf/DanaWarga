using System.Net.Http.Headers;
using System.Net.Http.Json;
using DanaWarga.Contracts.Auth;
using DanaWarga.Contracts.Houses;
using DanaWarga.Contracts.Payments;
using DanaWarga.Contracts.Reports;
using DanaWarga.Contracts.Residents;

namespace DanaWarga.WebMvc.Services;

public sealed class ApiGateway(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("DanaWargaApi");
        var response = await client.PostAsJsonAsync("api/auth/login", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken))!;
    }

    public async Task<IReadOnlyCollection<ResidentDto>> GetResidentsAsync(CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        return await client.GetFromJsonAsync<IReadOnlyCollection<ResidentDto>>("api/residents", cancellationToken) ?? [];
    }

    public async Task CreateResidentAsync(CreateResidentRequest request, CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync("api/residents", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyCollection<HouseDto>> GetHousesAsync(CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        return await client.GetFromJsonAsync<IReadOnlyCollection<HouseDto>>("api/houses", cancellationToken) ?? [];
    }

    public async Task CreateHouseAsync(CreateHouseRequest request, CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync("api/houses", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyCollection<IplPaymentDto>> GetPaymentsAsync(CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        return await client.GetFromJsonAsync<IReadOnlyCollection<IplPaymentDto>>("api/ipl-payments", cancellationToken) ?? [];
    }

    public async Task SubmitPaymentAsync(SubmitIplPaymentRequest request, CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync("api/ipl-payments", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task ValidatePaymentAsync(Guid paymentId, bool approve, CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync($"api/ipl-payments/{paymentId}/validate", new ValidatePaymentRequest(approve), cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IplMatrixReportDto?> GetIplMatrixAsync(int year, CancellationToken cancellationToken)
    {
        var client = CreateAuthorizedClient();
        return await client.GetFromJsonAsync<IplMatrixReportDto>($"api/reports/ipl-matrix?year={year}", cancellationToken);
    }

    private HttpClient CreateAuthorizedClient()
    {
        var client = httpClientFactory.CreateClient("DanaWargaApi");
        var token = httpContextAccessor.HttpContext?.Session.GetString("access_token");
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }
}