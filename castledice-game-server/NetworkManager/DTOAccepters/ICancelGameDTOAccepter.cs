using casltedice_events_logic.ClientToServer;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface ICancelGameDTOAccepter
{
    void AcceptCancelGameDTO(CancelGameDTO dto, ushort clientId);
}