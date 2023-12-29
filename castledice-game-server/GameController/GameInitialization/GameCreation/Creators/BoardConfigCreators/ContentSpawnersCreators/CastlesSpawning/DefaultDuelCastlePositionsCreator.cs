using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public class DefaultDuelCastlePositionsCreator : IDuelCastlesPositionsCreator
{
    private readonly Vector2Int _firstCastlePosition;
    private readonly Vector2Int _secondCastlePosition;

    public DefaultDuelCastlePositionsCreator(Vector2Int firstCastlePosition, Vector2Int secondCastlePosition)
    {
        _firstCastlePosition = firstCastlePosition;
        _secondCastlePosition = secondCastlePosition;
    }

    public Vector2Int GetFirstCastlePosition()
    {
        return _firstCastlePosition;
    }

    public Vector2Int GetSecondCastlePosition()
    {
        return _secondCastlePosition;
    }
}