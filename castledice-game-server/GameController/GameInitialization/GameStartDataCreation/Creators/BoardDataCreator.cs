using castledice_game_data_logic.ConfigsData;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

public class BoardDataCreator : IBoardDataCreator
{
    private readonly ICellsPresenceMatrixCreator _cellsPresenceMatrixCreator;
    private readonly IContentDataListCreator _contentDataListCreator;

    public BoardDataCreator(ICellsPresenceMatrixCreator cellsPresenceMatrixCreator, IContentDataListCreator contentDataListCreator)
    {
        _cellsPresenceMatrixCreator = cellsPresenceMatrixCreator;
        _contentDataListCreator = contentDataListCreator;
    }

    public BoardData GetBoardData(Board board)
    {
        var boardLength = board.GetLength(0);
        var boardWidth = board.GetLength(1);
        var cellType = board.GetCellType();
        var cellsPresence = _cellsPresenceMatrixCreator.GetCellsPresenceMatrix(board);
        var contentDataList = _contentDataListCreator.GetContentDataList(board);
        return new BoardData(boardLength, boardWidth, cellType, cellsPresence, contentDataList);
    }
}