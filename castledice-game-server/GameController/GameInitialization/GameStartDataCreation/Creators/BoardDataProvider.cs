using castledice_game_data_logic.ConfigsData;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public class BoardDataProvider : IBoardDataProvider
{
    private readonly ICellsPresenceMatrixProvider _cellsPresenceMatrixProvider;
    private readonly IContentDataListProvider _contentDataListProvider;

    public BoardDataProvider(ICellsPresenceMatrixProvider cellsPresenceMatrixProvider, IContentDataListProvider contentDataListProvider)
    {
        _cellsPresenceMatrixProvider = cellsPresenceMatrixProvider;
        _contentDataListProvider = contentDataListProvider;
    }

    public BoardData GetBoardData(Board board)
    {
        var boardLength = board.GetLength(0);
        var boardWidth = board.GetLength(1);
        var cellType = board.GetCellType();
        var cellsPresence = _cellsPresenceMatrixProvider.GetCellsPresenceMatrix(board);
        var contentDataList = _contentDataListProvider.GetContentDataList(board);
        return new BoardData(boardLength, boardWidth, cellType, cellsPresence, contentDataList);
    }
}