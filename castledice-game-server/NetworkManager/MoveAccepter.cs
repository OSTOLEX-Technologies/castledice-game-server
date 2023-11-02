using casltedice_events_logic.ClientToServer;
using castledice_game_server.GameController.Moves;
using castledice_game_server.NetworkManager.DTOAccepters;

namespace castledice_game_server.NetworkManager;

public class MoveAccepter : IMoveFromClientDTOAccepter
{
    private readonly IMovesController _controller;
    
    public MoveAccepter(IMovesController controller)
    {
        _controller = controller;
    } 
    
    public void AcceptMoveFromClientDTO(MoveFromClientDTO dto)
    {
        _controller.MakeMove(dto.MoveData);
    }
}