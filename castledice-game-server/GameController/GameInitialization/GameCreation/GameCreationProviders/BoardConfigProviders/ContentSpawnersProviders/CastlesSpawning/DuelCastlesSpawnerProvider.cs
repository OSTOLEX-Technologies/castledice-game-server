using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public class DuelCastlesSpawnerProvider : ICastlesSpawnerProvider
{
    private readonly IDuelCastlesPositionsProvider _positionsProvider;
    private readonly ICastlesFactoryProvider _factoryProvider;

    public DuelCastlesSpawnerProvider(IDuelCastlesPositionsProvider positionsProvider, ICastlesFactoryProvider factoryProvider)
    {
        _positionsProvider = positionsProvider;
        _factoryProvider = factoryProvider;
    }

    public CastlesSpawner GetCastlesSpawner(List<Player> players)
    {
        throw new NotImplementedException();
    }
}