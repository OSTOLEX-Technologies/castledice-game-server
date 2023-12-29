using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public interface ICellsPresenceMatrixProvider
{
    bool[,] GetCellsPresenceMatrix(Board board);
}