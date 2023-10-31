using castledice_game_data_logic;

namespace castledice_game_server.GameService;

public interface IHttpGameDataSender
{
    Task<GameData> SendGameDataAsync(GameData gameData, HttpMethod method);
}