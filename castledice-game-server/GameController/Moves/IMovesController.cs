using castledice_game_data_logic.Moves;

namespace castledice_game_server.GameController.Moves;

public interface IMovesController
{
    void MakeMove(MoveData moveData);
}