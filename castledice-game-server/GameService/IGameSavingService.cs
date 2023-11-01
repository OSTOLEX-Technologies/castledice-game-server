using castledice_game_data_logic;

namespace castledice_game_server.GameService;

public interface IGameSavingService
{
    Task<int> SaveGameStartAsync(GameStartData gameStartData);
    Task SaveGameEndAsync(int gameId, string history, int? winnerId = null);
}