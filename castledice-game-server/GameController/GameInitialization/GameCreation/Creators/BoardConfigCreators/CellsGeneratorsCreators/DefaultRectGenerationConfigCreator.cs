namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.CellsGeneratorsCreators;

public class DefaultRectGenerationConfigCreator : IRectGenerationConfigCreator
{
    private readonly int _boardWidth;
    private readonly int _boardLength;

    public DefaultRectGenerationConfigCreator(int boardWidth, int boardLength)
    {
        _boardWidth = boardWidth;
        _boardLength = boardLength;
    }

    public RectGenerationConfig GetRectGenerationConfig()
    {
        return new RectGenerationConfig(_boardWidth, _boardLength);
    }
}