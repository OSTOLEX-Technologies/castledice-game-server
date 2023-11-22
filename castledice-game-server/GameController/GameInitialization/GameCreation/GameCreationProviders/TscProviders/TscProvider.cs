using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic.TurnsLogic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.ActionPointsConditionProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public class TscProvider : ITscProvider
{
    private readonly IActionPointsConditionProvider _actionPointsConditionProvider;
    private readonly ITimeConditionProvider _timeConditionProvider;
    
    public TscProvider(IActionPointsConditionProvider actionPointsConditionProvider, ITimeConditionProvider timeConditionProvider)
    {
        _actionPointsConditionProvider = actionPointsConditionProvider;
        _timeConditionProvider = timeConditionProvider;
    }
    
    public ITurnSwitchCondition GetTurnSwitchCondition(TscType type)
    {
        return type switch
        {
            TscType.Time => _timeConditionProvider.GetTimeCondition(),
            TscType.ActionPoints => _actionPointsConditionProvider.GetActionPointsCondition(),
            _ => throw new ArgumentException("Unknown TscType: " + type)
        };
    }
}