using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

public interface ITimeConditionProvider
{
    TimeCondition GetTimeCondition();
}