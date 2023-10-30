namespace castledice_game_server.GameController.PlayerInitialization;

public interface IPlayerInitializationController
{
    Task InitializePlayerAsync(string playerToken, ushort clientId);
}