namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public interface IPlayerDisconnectedSender
{
    public void SendPlayerDisconnectedMessage(int disconnectedPlayerId, int sendToPlayerId);
}