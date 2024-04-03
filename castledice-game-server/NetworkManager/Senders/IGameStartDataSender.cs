using castledice_game_data_logic;

namespace castledice_game_server.NetworkManager.Senders;

public interface IGameStartDataSender
{
    void SendGameStartData(GameStartData data);
}