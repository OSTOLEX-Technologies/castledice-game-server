using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameDataSaver;

public interface IGameSavingService
{
    Task<int> SaveGameStartAsync(GameStartData gameStartData);
    Task SaveGameEndAsync(int gameId, int winnerId, string history);
}