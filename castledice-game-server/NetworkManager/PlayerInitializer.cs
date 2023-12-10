using castledice_events_logic.ClientToServer;
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

    public async Task AcceptInitializePlayerDTOAsync(InitializePlayerDTO dto, ushort clientId)
    {
        await _controller.InitializePlayerAsync(dto.VerificationKey, clientId);
    }
}