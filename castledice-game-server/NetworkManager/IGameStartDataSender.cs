using castledice_game_data_logic;

namespace castledice_game_server.NetworkManager;

public interface IGameStartDataSender
{
    void SendGameStartData(GameStartData data);
}