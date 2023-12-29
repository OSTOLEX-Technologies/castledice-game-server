using castledice_game_logic.GameObjects.Configs;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;

public class DefaultKnightConfigCreator : IKnightConfigCreator
{
    private readonly int _placementCost;
    private readonly int _health;

    public DefaultKnightConfigCreator(int placementCost, int health)
    {
        _placementCost = placementCost;
        _health = health;
    }

    public KnightConfig GetKnightConfig()
    {
        return new KnightConfig(_placementCost, _health);
    }
}