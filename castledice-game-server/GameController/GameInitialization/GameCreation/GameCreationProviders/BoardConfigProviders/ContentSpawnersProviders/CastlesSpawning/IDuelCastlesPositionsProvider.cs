using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public interface IDuelCastlesPositionsProvider
{
    Vector2Int GetFirstCastlePosition();
    Vector2Int GetSecondCastlePosition();
}