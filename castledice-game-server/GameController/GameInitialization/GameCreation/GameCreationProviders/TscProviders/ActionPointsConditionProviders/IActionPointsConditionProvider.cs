using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.ActionPointsConditionProviders;

public interface IActionPointsConditionProvider
{
    ActionPointsCondition GetActionPointsCondition();
}