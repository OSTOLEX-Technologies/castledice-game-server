using System.Text;
using castledice_game_data_logic;
using castledice_game_server.HttpUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace castledice_game_server.GameRepository;

public class HttpGameDataRepository : IHttpGameDataRepository
{
    private readonly string _storageUrl;
    private readonly IHttpMessageSender _httpMessageSender;

    public HttpGameDataRepository(string storageUrl, IHttpMessageSender httpMessageSender)
    {
        _storageUrl = storageUrl;
        _httpMessageSender = httpMessageSender;
    }

    public async Task<GameData> PostGameDataAsync(GameData data)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _storageUrl + "/game");
        requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        using var response = await _httpMessageSender.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return GetGameDataFromPostResponse(responseBody);

    }

    private static GameData GetGameDataFromPostResponse(string responseBody)
    {
        var data = JObject.Parse(responseBody); //Parsing response body
        if (data.TryGetValue("game", out var gameToken))
        {
            var gameData = JsonConvert.DeserializeObject<GameData>(gameToken.ToString());
            if (gameData is null)
            {
                throw new InvalidOperationException("Cannot deserialize game data from response body");
            }
            return gameData;
        }
        throw new InvalidOperationException("Response body does not contain game field");
    }

    public async Task<GameData> GetGameDataAsync(int gameId)
    {
        throw new NotImplementedException();
    }

    public async Task<GameData> PutGameDataAsync(GameData data)
    {
        throw new NotImplementedException();
    }
}