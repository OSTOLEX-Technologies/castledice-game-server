using castledice_game_data_logic;
using castledice_game_server.Exceptions;
using castledice_game_server.GameService;

namespace castledice_game_server.Stubs;

public class GameSavingServiceWithErrorStub : IGameSavingService
{
    public int ThrowErrorDelay { get; set; } = 100;
    
    public async Task<int> SaveGameStartAsync(GameStartData gameStartData)
    {
        await Task.Delay(ThrowErrorDelay);
        throw new GameNotSavedException();
    }

    public async Task SaveGameEndAsync(int gameId, string history, int? winnerId = null)
    {
        await Task.Delay(ThrowErrorDelay);
        throw new GameNotSavedException();
    }
}