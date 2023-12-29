using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;

public class BoardConfigCreator : IBoardConfigCreator
{
    private readonly ICellsGeneratorCreator _cellsGeneratorCreator;
    private readonly IContentSpawnersListCreator _contentSpawnersListCreator;

    public BoardConfigCreator(ICellsGeneratorCreator cellsGeneratorCreator, IContentSpawnersListCreator contentSpawnersListCreator)
    {
        _cellsGeneratorCreator = cellsGeneratorCreator;
        _contentSpawnersListCreator = contentSpawnersListCreator;
    }

    public BoardConfig GetBoardConfig(List<Player> players)
    {
        var cellType = CellType.Square;
        var cellsGenerator = _cellsGeneratorCreator.GetCellsGenerator();
        var contentSpawners = _contentSpawnersListCreator.GetContentSpawners(players);
        return new BoardConfig(contentSpawners, cellsGenerator, cellType);
    }
}