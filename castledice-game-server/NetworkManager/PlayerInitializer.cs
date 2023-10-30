using casltedice_events_logic.ClientToServer;
using castledice_game_server.GameController.PlayerInitialization;
using castledice_game_server.NetworkManager.DTOAccepters;

namespace castledice_game_server.NetworkManager;

public class PlayerInitializer : IInitializePlayerDTOAccepter
{
    private readonly IPlayerInitializationController _controller;

    public PlayerInitializer(IPlayerInitializationController controller)
    {
        _controller = controller;
    }

    public void AcceptInitializePlayerDTO(InitializePlayerDTO dto, ushort clientId)
    {
        _controller.InitializePlayerAsync(dto.VerificationKey, clientId);
    }
}