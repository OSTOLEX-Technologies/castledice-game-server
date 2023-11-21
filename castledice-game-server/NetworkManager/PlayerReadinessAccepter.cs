using casltedice_events_logic.ClientToServer;
using castledice_game_server.GameController.PlayersReadiness;
using castledice_game_server.NetworkManager.DTOAccepters;

namespace castledice_game_server.NetworkManager;

public class PlayerReadinessAccepter : IPlayerReadyDTOAccepter
{
    private readonly IPlayersReadinessController _controller;

    public PlayerReadinessAccepter(IPlayersReadinessController controller)
    {
        _controller = controller;
    }

    public async Task AcceptPlayerReadyDTOAsync(PlayerReadyDTO playerReadyDTO)
    {
        await _controller.SetPlayerReadyAsync(playerReadyDTO.VerificationKey);
    }
}