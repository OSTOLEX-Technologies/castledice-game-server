namespace castledice_game_server.GameController.PlayerInitialization;

public interface IInitializationResultSender
{
    void SendInitializationResult(ushort clientId, bool success); 
}