using casltedice_events_logic.ClientToServer;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface IPlayerReadyDTOAccepter
{
    Task AcceptPlayerReadyDTOAsync(PlayerReadyDTO playerReadyDTO);
}