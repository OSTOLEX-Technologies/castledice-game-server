using castledice_game_data_logic;
using castledice_game_server.GameService;

namespace castledice_game_server.Stubs;

/// <summary>
/// This class MUST NOT be used in production build.
/// </summary>
public class GameSavingServiceStub : IGameSavingService
{
    private readonly List<int> _gameIds = new();
    
    public async Task<int> SaveGameStartAsync(GameStartData gameStartData)
    {
        var lastId = _gameIds.LastOrDefault();
        var newId = lastId + 1;
        _gameIds.Add(newId);
        return newId;
    }

    public async Task SaveGameEndAsync(int gameId, string history, int? winnerId = null)
    {
        _gameIds.Remove(gameId);
    }
}