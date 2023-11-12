namespace castledice_game_server.GameController.PlayersReadiness;

public class PlayersReadinessTracker : IPlayersReadinessTracker
{
    private readonly Dictionary<int, bool> _playersReadiness = new();
    
    public void SetPlayerReadiness(int playerId, bool isPlayerReady)
    {
        _playersReadiness[playerId] = isPlayerReady;
    }

    public bool IsPlayerReady(int playerId)
    {
        return _playersReadiness.TryGetValue(playerId, out var isPlayerReady) && isPlayerReady;
    }
}