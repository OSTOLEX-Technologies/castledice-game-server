namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;

public class DefaultRectGenerationConfigProvider : IRectGenerationConfigProvider
{
    private readonly int _boardWidth;
    private readonly int _boardLength;

    public DefaultRectGenerationConfigProvider(int boardWidth, int boardLength)
    {
        _boardWidth = boardWidth;
        _boardLength = boardLength;
    }

    public RectGenerationConfig GetRectGenerationConfig()
    {
        throw new NotImplementedException();
    }
}