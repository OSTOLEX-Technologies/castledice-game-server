using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameDataSaver;

public interface IGameSavingService
{
    void SaveGameStart(GameStartData gameStartData);
    void SaveGameEnd(Game game);
}