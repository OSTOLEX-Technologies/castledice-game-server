using castledice_game_logic.Time;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public class StopwatchPlayerTimerCreator : IPlayerTimerCreator
{
    private readonly IPlayerTimeSpanCreator _playerTimeSpanCreator;

    public StopwatchPlayerTimerCreator(IPlayerTimeSpanCreator playerTimeSpanCreator)
    {
        _playerTimeSpanCreator = playerTimeSpanCreator;
    }

    public IPlayerTimer GetPlayerTimer()
    {
        var timeSpan = _playerTimeSpanCreator.GetTimeSpan();
        return new StopwatchPlayerTimer(timeSpan);
    }
}