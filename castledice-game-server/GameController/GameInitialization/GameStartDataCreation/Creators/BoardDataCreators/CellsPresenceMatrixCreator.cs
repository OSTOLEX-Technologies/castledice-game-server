using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.BoardDataCreators;

public class CellsPresenceMatrixCreator : ICellsPresenceMatrixCreator
{
    public bool[,] GetCellsPresenceMatrix(Board board)
    {
        var boardLength = board.GetLength(0);
        var boardWidth = board.GetLength(1);
        var cellsPresence = new bool[boardLength, boardWidth];
        foreach (var cell in board)
        {
            cellsPresence[cell.Position.X, cell.Position.Y] = true;
        }
        return cellsPresence;
    }
}