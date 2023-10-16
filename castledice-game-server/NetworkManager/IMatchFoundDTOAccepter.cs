using casltedice_events_logic.ServerToClient;

namespace castledice_game_server.NetworkManager;

public interface IMatchFoundDTOAccepter
{
    void AcceptMatchFoundDTO(MatchFoundDTO matchFoundDTO);
}