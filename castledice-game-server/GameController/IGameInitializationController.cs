namespace castledice_game_server.GameController;

public interface IGameInitializationController
{
    void InitializeGame(List<int> playersIds);
}