using castledice_game_data_logic.Errors;

namespace castledice_game_server.GameController;

public interface IErrorSender
{
    void SendErrorToPlayer(ErrorData errorData, int playerId);
}