namespace castledice_game_server.GameController.GameInitialization;

public interface IGameInitializationController
{
    void InitializeGame(List<int> playersIds);
}