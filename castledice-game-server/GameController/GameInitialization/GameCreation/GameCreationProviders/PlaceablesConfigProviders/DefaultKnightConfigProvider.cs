using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;

public class DefaultKnightConfigProvider : IKnightConfigProvider
{
    private readonly int _placementCost;
    private readonly int _health;

    public DefaultKnightConfigProvider(int placementCost, int health)
    {
        _placementCost = placementCost;
        _health = health;
    }

    public KnightConfig GetKnightConfig()
    {
        return new KnightConfig(_placementCost, _health);
    }
}