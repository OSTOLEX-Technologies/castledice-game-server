using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

public class TscListProvider : ITscListProvider
{
    private readonly ITscPresenceConfigProvider _presenceConfigProvider;
    private readonly ITscProvider _tscProvider;

    public TscListProvider(ITscPresenceConfigProvider presenceConfigProvider, ITscProvider tscProvider)
    {
        _presenceConfigProvider = presenceConfigProvider;
        _tscProvider = tscProvider;
    }

    public List<ITurnSwitchCondition> GetTurnSwitchConditions()
    {
        var tscPresenceConfig = _presenceConfigProvider.GetTscPresenceConfig();
        var tscList = new List<ITurnSwitchCondition>();
        foreach (var tscType in tscPresenceConfig.PresentConditions)
        {
            tscList.Add(_tscProvider.GetTurnSwitchCondition(tscType));
        }

        return tscList;
    }
}