namespace castledice_game_server.GameController.GameInitialization;

public interface IGameInitializationController
{
    Task InitializeGameAsync(List<int> playersIds);
}