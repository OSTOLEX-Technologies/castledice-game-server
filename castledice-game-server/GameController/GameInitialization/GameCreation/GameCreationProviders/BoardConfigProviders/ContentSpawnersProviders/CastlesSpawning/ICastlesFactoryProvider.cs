using castledice_game_logic.GameObjects.Factories;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public interface ICastlesFactoryProvider
{
    ICastlesFactory GetCastlesFactory();
}