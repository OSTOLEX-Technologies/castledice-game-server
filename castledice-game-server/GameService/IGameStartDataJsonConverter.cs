using castledice_game_data_logic;

namespace castledice_game_server.GameService;

public interface IGameStartDataJsonConverter
{
    string GetJson(GameStartData data);
}