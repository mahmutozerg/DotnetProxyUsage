namespace WebScrapping.Service.Services;

public class RequestService
{
    private readonly HttpClient _httpClient;

    public RequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendRequestAsync(string url)
    {
        AddHttpPrefixIfNotExist(ref url);
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    private void AddHttpPrefixIfNotExist(ref string url)
    {
        if (!url.Contains("https://"))
        {
            url = "https://" + url;
        }
        
    }
}