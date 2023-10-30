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
        throw new NotImplementedException();
    }
}