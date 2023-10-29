using castledice_game_logic.BoardGeneration.CellsGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;

public class RectCellsGeneratorProvider : ICellsGeneratorProvider
{
    private readonly IRectGenerationConfigProvider _rectGenerationConfigProvider;

    public RectCellsGeneratorProvider(IRectGenerationConfigProvider rectGenerationConfigProvider)
    {
        _rectGenerationConfigProvider = rectGenerationConfigProvider;
    }

    public ICellsGenerator GetCellsGenerator()
    {
        var config = _rectGenerationConfigProvider.GetRectGenerationConfig();
        return new RectCellsGenerator(config.BoardLength, config.BoardWidth);
    }
}