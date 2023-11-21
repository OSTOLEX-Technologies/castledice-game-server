using castledice_game_data_logic.TurnSwitchConditions;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public class TscPresenceConfig
{
    public IReadOnlyCollection<TscType> PresentTypes => _presentTypes;
    private readonly HashSet<TscType> _presentTypes;

    public TscPresenceConfig(HashSet<TscType> presentTypes)
    {
        _presentTypes = presentTypes;
    }
}