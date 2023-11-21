using castledice_game_data_logic;

namespace castledice_game_server.GameRepository;

public interface ILocalGameDataRepository
{
    void AddGameData(GameData data);
    GameData GetGameData(int id);
    bool RemoveGameData(int id);
}