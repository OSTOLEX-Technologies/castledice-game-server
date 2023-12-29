using castledice_game_logic.BoardGeneration.CellsGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;

public class RectCellsGeneratorCreator : ICellsGeneratorCreator
{
    private readonly IRectGenerationConfigCreator _rectGenerationConfigCreator;

    public RectCellsGeneratorCreator(IRectGenerationConfigCreator rectGenerationConfigCreator)
    {
        _rectGenerationConfigCreator = rectGenerationConfigCreator;
    }

    public ICellsGenerator GetCellsGenerator()
    {
        var config = _rectGenerationConfigCreator.GetRectGenerationConfig();
        return new RectCellsGenerator(config.BoardLength, config.BoardWidth);
    }
}