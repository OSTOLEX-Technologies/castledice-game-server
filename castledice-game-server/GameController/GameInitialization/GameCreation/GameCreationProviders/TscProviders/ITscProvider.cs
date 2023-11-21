using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public interface ITscProvider
{
    ITurnSwitchCondition GetTurnSwitchCondition(TscType type);
}