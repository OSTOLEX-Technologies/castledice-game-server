using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic.GameConfiguration;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;

public class TscConfigDataCreator : ITscConfigDataCreator
{
    public TscConfigData GetTscConfigData(TurnSwitchConditionsConfig turnSwitchConditionsConfig)
    {
        return new TscConfigData(turnSwitchConditionsConfig.ConditionsToUse);
    }
}