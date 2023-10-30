using System.Net.Http.Headers;
using castledice_game_server.HttpUtilities;

namespace castledice_game_server.Auth;

public class HttpIdRetriever : IIdRetriever
{
    private readonly string _authServiceUrl;
    private readonly IHttpMessageSender _messageSender;

    public HttpIdRetriever(string authServiceUrl, IHttpMessageSender messageSender)
    {
        _authServiceUrl = authServiceUrl;
        _messageSender = messageSender;
    }

    public Task<int> RetrievePlayerIdAsync(string playerToken)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, _authServiceUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", playerToken);
        requestMessage.RequestUri.ToString();
        using var response = await _messageSender.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return GetIdFromJson(responseBody);
    }
}