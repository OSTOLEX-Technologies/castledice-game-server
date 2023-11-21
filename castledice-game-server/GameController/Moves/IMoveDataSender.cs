using castledice_game_data_logic.Moves;

namespace castledice_game_server.GameController.Moves;

public interface IMoveDataSender
{
    void SendDataToPlayer(MoveData moveData, int playerId);
}