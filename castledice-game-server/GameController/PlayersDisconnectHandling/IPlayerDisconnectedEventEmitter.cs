namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public interface IPlayerDisconnectedEventEmitter
{
    public event Action<int> PlayerDisconnected;
}