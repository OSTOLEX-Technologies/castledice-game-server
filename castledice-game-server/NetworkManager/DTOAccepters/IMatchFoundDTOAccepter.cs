using castledice_events_logic.ServerToClient;

namespace castledice_game_server.NetworkManager.DTOAccepters;

public interface IMatchFoundDTOAccepter
{
    Task AcceptMatchFoundDTOAsync(MatchFoundDTO matchFoundDTO);
}