using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Settings;

namespace SafeRoad.Infrastructure.Services;

public class SupabaseStorageService : IBlobStorageService
{
    private readonly HttpClient _httpClient;
    private readonly SupabaseStorageSettings _settings;

    public SupabaseStorageService(HttpClient httpClient, IOptions<SupabaseStorageSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;

        _httpClient.BaseAddress = new Uri(_settings.SupabaseUrl.TrimEnd('/') + "/storage/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", _settings.SupabaseServiceKey);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.SupabaseServiceKey);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var uniqueName = $"{Guid.NewGuid():N}_{fileName}";
        var requestUri = $"object/{_settings.BucketName}/{uniqueName}";

        using var content = new StreamContent(fileStream);
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        var response = await _httpClient.PostAsync(requestUri, content);
        response.EnsureSuccessStatusCode();

        // Return the public URL
        return $"{_settings.SupabaseUrl.TrimEnd('/')}/storage/v1/object/public/{_settings.BucketName}/{uniqueName}";
    }

    public async Task DeleteAsync(string blobUrl)
    {
        if (string.IsNullOrWhiteSpace(blobUrl)) return;

        // Extract file path from the full URL
        var prefix = $"/storage/v1/object/public/{_settings.BucketName}/";
        var idx = blobUrl.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return;

        var filePath = blobUrl[(idx + prefix.Length)..];
        var requestUri = $"object/{_settings.BucketName}/{filePath}";

        var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
