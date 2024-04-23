using castledice_events_logic.ServerToClient;

namespace castledice_game_server.GameController.PlayerInitialization;

public interface IPlayerInitializationResultDTOSender
{
    void SendInitializationResult(ushort clientId, PlayerInitializationResultDTO dto); 
}