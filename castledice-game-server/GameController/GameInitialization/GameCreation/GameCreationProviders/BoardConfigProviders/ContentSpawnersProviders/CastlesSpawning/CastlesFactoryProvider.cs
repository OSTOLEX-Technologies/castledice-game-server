using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.GameObjects.Factories.Castles;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public class CastlesFactoryProvider : ICastlesFactoryProvider
{
    private readonly ICastleConfigProvider _configProvider;

    public CastlesFactoryProvider(ICastleConfigProvider configProvider)
    {
        _configProvider = configProvider;
    }

    public ICastlesFactory GetCastlesFactory()
    {
        return new CastlesFactory(_configProvider.GetCastleConfig());
    }
}