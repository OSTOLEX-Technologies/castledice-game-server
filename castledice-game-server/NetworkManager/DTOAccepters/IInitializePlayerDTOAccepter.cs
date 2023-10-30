using casltedice_events_logic.ClientToServer;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface IInitializePlayerDTOAccepter
{
    void AcceptInitializePlayerDTO(InitializePlayerDTO dto, ushort clientId);
}