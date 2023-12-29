using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;

public class DefaultTscConfigCreator : ITscConfigCreator
{
    private readonly List<TscType> _tscTypesToUse;
    
    public DefaultTscConfigCreator(List<TscType> tscTypesToUse)
    {
        _tscTypesToUse = tscTypesToUse;
    }

    public TurnSwitchConditionsConfig GetTurnSwitchConditionsConfig()
    {
        return new TurnSwitchConditionsConfig(_tscTypesToUse);
    }
}