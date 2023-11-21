using castledice_game_data_logic;

namespace castledice_game_server.GameRepository;

public interface IHttpGameDataRepository
{
    Task<GameData> PostGameDataAsync(GameData data);
    Task<GameData> GetGameDataAsync(int gameId);
    Task<GameData> PutGameDataAsync(GameData data);
}