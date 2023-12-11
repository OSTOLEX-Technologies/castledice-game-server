using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscConfigProviders;

public class DefaultTscConfigProvider : ITscConfigProvider
{
    private readonly List<TscType> _tscTypesToUse;
    
    public DefaultTscConfigProvider(List<TscType> tscTypesToUse)
    {
        _tscTypesToUse = tscTypesToUse;
    }

    public TurnSwitchConditionsConfig GetTurnSwitchConditionsConfig()
    {
        return new TurnSwitchConditionsConfig(_tscTypesToUse);
    }
}