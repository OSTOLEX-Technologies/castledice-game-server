using casltedice_events_logic.ClientToServer;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface IRequestGameDTOAccepter
{
    void AcceptRequestGameDTO(RequestGameDTO requestGameDTO, ushort clientId);
}