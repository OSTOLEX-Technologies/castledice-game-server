using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;

public class BoardConfigProvider : IBoardConfigProvider
{
    private readonly ICellsGeneratorProvider _cellsGeneratorProvider;
    private readonly IContentSpawnersListProvider _contentSpawnersListProvider;

    public BoardConfigProvider(ICellsGeneratorProvider cellsGeneratorProvider, IContentSpawnersListProvider contentSpawnersListProvider)
    {
        _cellsGeneratorProvider = cellsGeneratorProvider;
        _contentSpawnersListProvider = contentSpawnersListProvider;
    }

    public BoardConfig GetBoardConfig(List<Player> players)
    {
        var cellType = CellType.Square;
        var cellsGenerator = _cellsGeneratorProvider.GetCellsGenerator();
        var contentSpawners = _contentSpawnersListProvider.GetContentSpawners(players);
        return new BoardConfig(contentSpawners, cellsGenerator, cellType);
    }
}