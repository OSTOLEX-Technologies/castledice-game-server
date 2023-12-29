using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;

public interface ITscConfigCreator
{
    TurnSwitchConditionsConfig GetTurnSwitchConditionsConfig();
}