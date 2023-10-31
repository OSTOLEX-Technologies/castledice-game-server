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
        throw new NotImplementedException();
    }

    public async Task SaveGameEndAsync(int gameId, int winnerId, string history)
    {
        throw new NotImplementedException();
    }
}