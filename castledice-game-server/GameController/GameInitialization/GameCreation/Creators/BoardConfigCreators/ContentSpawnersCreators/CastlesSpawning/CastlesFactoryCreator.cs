using castledice_game_logic.GameObjects.Factories;
using castledice_game_logic.GameObjects.Factories.Castles;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public class CastlesFactoryCreator : ICastlesFactoryCreator
{
    private readonly ICastleConfigCreator _configCreator;

    public CastlesFactoryCreator(ICastleConfigCreator configCreator)
    {
        _configCreator = configCreator;
    }

    public ICastlesFactory GetCastlesFactory()
    {
        return new CastlesFactory(_configCreator.GetCastleConfig());
    }
}