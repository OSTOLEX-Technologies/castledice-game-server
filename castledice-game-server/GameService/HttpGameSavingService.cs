using castledice_game_data_logic;
using castledice_game_server.Exceptions;
using castledice_game_server.GameRepository;

namespace castledice_game_server.GameService;

public class HttpGameSavingService : IGameSavingService
{
    private readonly IHttpGameDataRepository _dataRepository;
    private readonly ICurrentTimeProvider _currentTimeProvider;
    private readonly IGameStartDataJsonConverter _gameStartDataJsonConverter;
    private readonly ILocalGameDataRepository _localGameDataRepository;

    public HttpGameSavingService(IHttpGameDataRepository dataRepository, ICurrentTimeProvider currentTimeProvider, IGameStartDataJsonConverter gameStartDataJsonConverter, ILocalGameDataRepository localGameDataRepository)
    {
        _dataRepository = dataRepository;
        _currentTimeProvider = currentTimeProvider;
        _gameStartDataJsonConverter = gameStartDataJsonConverter;
        _localGameDataRepository = localGameDataRepository;
    }

    public async Task<int> SaveGameStartAsync(GameStartData gameStartData)
    {
        var config = _gameStartDataJsonConverter.GetJson(gameStartData);
        var startTime = _currentTimeProvider.GetCurrentTime();
        var gameData = new GameData(0, config, startTime, gameStartData.PlayersIds);
        try
        {
            var responseData = await _dataRepository.PostGameDataAsync(gameData);
            _localGameDataRepository.AddGameData(responseData);
            return responseData.Id;
        }
        catch (HttpRequestException e)
        {
            throw new GameNotSavedException("Could not save game start data.", e);
        }
    }

    public async Task SaveGameEndAsync(int gameId, int winnerId, string history)
    {
        GameData gameData;
        try
        {
            gameData = await GetGameData(gameId);
        }
        catch (GameDataNotFoundException e)
        {
            throw new GameNotSavedException("Could not save game end data.", e);
        }
        gameData.WinnerId = winnerId;
        gameData.History = history;
        gameData.GameEndedTime = _currentTimeProvider.GetCurrentTime();
        try
        {
            await _dataRepository.PutGameDataAsync(gameData);
            _localGameDataRepository.RemoveGameData(gameData.Id);
        }
        catch (HttpRequestException e)
        {
            throw new GameNotSavedException("Could not save game end data.", e);
        }
    }

    private async Task<GameData> GetGameData(int gameId)
    {
        try
        {
            return await _dataRepository.GetGameDataAsync(gameId);
        }
        catch (HttpRequestException)
        {
            return _localGameDataRepository.GetGameData(gameId);
        }
    }
}