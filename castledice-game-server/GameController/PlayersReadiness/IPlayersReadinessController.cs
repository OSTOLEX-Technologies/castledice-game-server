namespace castledice_game_server.GameController.PlayersReadiness;

public interface IPlayersReadinessController
{
    Task SetPlayerReadyAsync(string token);
}