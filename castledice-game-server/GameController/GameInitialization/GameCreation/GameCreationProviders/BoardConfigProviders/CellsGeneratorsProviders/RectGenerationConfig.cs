namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;

public class RectGenerationConfig
{
    public int BoardWidth { get; }
    public int BoardLength { get; }

    public RectGenerationConfig(int boardWidth, int boardLength)
    {
        BoardWidth = boardWidth;
        BoardLength = boardLength;
    }
}