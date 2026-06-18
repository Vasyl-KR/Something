using Newtonsoft.Json;
using System.Text;
using Tests.Core.Helpers;

namespace Tests.Core.Clients;

public class ApiClient : IDisposable
{
    private readonly HttpClient _client;

    public ApiClient(string baseUrl)
    {
        _client = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        Log.Debug($"GET {endpoint}");
        var response = await _client.GetAsync(endpoint);
        Log.Debug($"GET {endpoint} → {(int)response.StatusCode}");
        return await DeserializeResponse<T>(response);
    }

    public async Task<T> PostAsync<T>(string endpoint, object payload)
    {
        Log.Debug($"POST {endpoint} | body: {JsonConvert.SerializeObject(payload)}");
        var content = Serialize(payload);
        var response = await _client.PostAsync(endpoint, content);
        Log.Debug($"POST {endpoint} → {(int)response.StatusCode}");
        return await DeserializeResponse<T>(response);
    }

    public async Task<T> PutAsync<T>(string endpoint, object payload)
    {
        Log.Debug($"PUT {endpoint} | body: {JsonConvert.SerializeObject(payload)}");
        var content = Serialize(payload);
        var response = await _client.PutAsync(endpoint, content);
        Log.Debug($"PUT {endpoint} → {(int)response.StatusCode}");
        return await DeserializeResponse<T>(response);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        Log.Debug($"DELETE {endpoint}");
        var response = await _client.DeleteAsync(endpoint);
        Log.Debug($"DELETE {endpoint} → {(int)response.StatusCode}");
        EnsureSuccess(response);
        return response;
    }

    public void AddHeader(string name, string value)
        => _client.DefaultRequestHeaders.Add(name, value);

    private static StringContent Serialize(object payload)
        => new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

    private static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
    {
        EnsureSuccess(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json)
            ?? throw new InvalidOperationException($"Failed to deserialize response to {typeof(T).Name}");
    }

    private static void EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            Log.Error($"Request failed: {(int)response.StatusCode} {response.ReasonPhrase} — {response.RequestMessage?.RequestUri}");
            throw new HttpRequestException(
                $"Request failed: {(int)response.StatusCode} {response.ReasonPhrase} — {response.RequestMessage?.RequestUri}");
        }
    }

    public void Dispose() => _client.Dispose();
}
