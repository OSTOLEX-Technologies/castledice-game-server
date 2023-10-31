using castledice_game_data_logic;

namespace castledice_game_server.GameService;

public class HttpGameSavingService : IGameSavingService
{
    private readonly IHttpGameDataSender _dataSender;
    private readonly ICurrentTimeProvider _currentTimeProvider;
    private readonly IGameStartDataJsonConverter _gameStartDataJsonConverter;

    public HttpGameSavingService(IHttpGameDataSender dataSender, ICurrentTimeProvider currentTimeProvider, IGameStartDataJsonConverter gameStartDataJsonConverter)
    {
        _dataSender = dataSender;
        _currentTimeProvider = currentTimeProvider;
        _gameStartDataJsonConverter = gameStartDataJsonConverter;
    }

    public async Task<int> SaveGameStartAsync(GameStartData gameStartData)
    {
        var config = _gameStartDataJsonConverter.GetJson(gameStartData);
        var startTime = _currentTimeProvider.GetCurrentTime();
        var gameData = new GameData(0, config, startTime, gameStartData.PlayersIds);
        var responseData = await _dataSender.SendGameDataAsync(gameData, HttpMethod.Post);
        return responseData.Id;
    }

    public async Task SaveGameEndAsync(int gameId, int winnerId, string history)
    {
        throw new NotImplementedException();
    }
}