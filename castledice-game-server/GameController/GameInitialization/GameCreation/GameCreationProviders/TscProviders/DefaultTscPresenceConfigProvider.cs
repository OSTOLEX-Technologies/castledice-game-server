using castledice_game_data_logic.TurnSwitchConditions;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public class DefaultTscPresenceConfigProvider : ITscPresenceConfigProvider
{
    private readonly HashSet<TscType> _presentConditions;

    public DefaultTscPresenceConfigProvider(HashSet<TscType> presentConditions)
    {
        _presentConditions = presentConditions;
    }

    public TscPresenceConfig GetTscPresenceConfig()
    {
        return new TscPresenceConfig(_presentConditions);
    }
}