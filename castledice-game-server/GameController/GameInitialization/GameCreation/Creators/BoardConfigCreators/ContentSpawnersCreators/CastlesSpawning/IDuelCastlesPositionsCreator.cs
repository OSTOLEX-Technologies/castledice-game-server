using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public interface IDuelCastlesPositionsCreator
{
    Vector2Int GetFirstCastlePosition();
    Vector2Int GetSecondCastlePosition();
}