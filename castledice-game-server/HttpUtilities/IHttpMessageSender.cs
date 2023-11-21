namespace castledice_game_server.HttpUtilities;

public interface IHttpMessageSender
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}