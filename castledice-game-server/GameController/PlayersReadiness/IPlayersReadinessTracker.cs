namespace castledice_game_server.GameController.PlayersReadiness;

public interface IPlayersReadinessTracker
{
    void SetPlayerReadiness(int playerId, bool isPlayerReady);
    bool PlayerIsReady(int playerId);
}