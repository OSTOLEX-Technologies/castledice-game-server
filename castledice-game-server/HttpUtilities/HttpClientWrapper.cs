namespace castledice_game_server.HttpUtilities;

public class HttpClientWrapper : IHttpMessageSender
{
    private readonly HttpClient _client;

    public HttpClientWrapper(HttpClient client)
    {
        _client = client;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return _client.SendAsync(request);
    }
}