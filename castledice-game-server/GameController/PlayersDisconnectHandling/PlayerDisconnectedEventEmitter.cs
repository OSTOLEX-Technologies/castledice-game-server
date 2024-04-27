namespace castledice_game_server.GameController.PlayersDisconnectHandling;

public class PlayerDisconnectedEventEmitter : IPlayerDisconnectedEventEmitter
{
    public event Action<int>? PlayerDisconnected;
    
    
}