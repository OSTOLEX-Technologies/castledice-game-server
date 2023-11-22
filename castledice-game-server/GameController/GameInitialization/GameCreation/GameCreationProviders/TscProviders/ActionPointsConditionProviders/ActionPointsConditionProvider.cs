using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.ActionPointsConditionProviders;

public class ActionPointsConditionProvider : IActionPointsConditionProvider
{
    private readonly ICurrentPlayerProvider _currentPlayerProvider;
    
    public ActionPointsConditionProvider(ICurrentPlayerProvider currentPlayerProvider)
    {
        _currentPlayerProvider = currentPlayerProvider;
    }
    
    public ActionPointsCondition GetActionPointsCondition()
    {
        return new ActionPointsCondition(_currentPlayerProvider);
    }
}