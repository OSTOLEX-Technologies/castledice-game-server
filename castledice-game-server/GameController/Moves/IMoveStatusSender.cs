namespace castledice_game_server.GameController.Moves;

public interface IMoveStatusSender
{
    void SendMoveStatusToPlayer(bool isApproved, int playerId);
}