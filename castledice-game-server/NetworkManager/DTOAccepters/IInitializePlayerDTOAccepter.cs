using casltedice_events_logic.ClientToServer;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface IInitializePlayerDTOAccepter
{
    Task AcceptInitializePlayerDTOAsync(InitializePlayerDTO dto, ushort clientId);
}