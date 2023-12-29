using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

public interface ICellsPresenceMatrixCreator
{
    bool[,] GetCellsPresenceMatrix(Board board);
}