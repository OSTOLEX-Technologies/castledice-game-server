using castledice_game_logic.Time;
using castledice_game_logic.TurnsLogic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders.TimeConditionProviders;

public class TimeConditionProvider : ITimeConditionProvider
{
    private readonly ITimeConditionConfigProvider _timeConditionConfigProvider;
    private readonly ITimer _timer;
    private readonly PlayerTurnsSwitcher _playerTurnsSwitcher;

    public TimeConditionProvider(ITimeConditionConfigProvider timeConditionConfigProvider, ITimer timer, PlayerTurnsSwitcher playerTurnsSwitcher)
    {
        _timeConditionConfigProvider = timeConditionConfigProvider;
        _timer = timer;
        _playerTurnsSwitcher = playerTurnsSwitcher;
    }

    public TimeCondition GetTimeCondition()
    {
        var config = _timeConditionConfigProvider.GetTimeConditionConfig();
        _timer.SetDuration(config.TurnDuration);
        return new TimeCondition(_timer, config.TurnDuration, _playerTurnsSwitcher);
    }
}