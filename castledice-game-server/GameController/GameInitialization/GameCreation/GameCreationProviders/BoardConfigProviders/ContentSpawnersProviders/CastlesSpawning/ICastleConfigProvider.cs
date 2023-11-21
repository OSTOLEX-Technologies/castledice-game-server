using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public interface ICastleConfigProvider
{
    CastleConfig GetCastleConfig();
}