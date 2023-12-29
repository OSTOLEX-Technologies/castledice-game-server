using castledice_game_logic.Time;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

public class StopwatchPlayerTimerProvider : IPlayerTimerProvider
{
    private readonly IPlayerTimeSpanProvider _playerTimeSpanProvider;

    public StopwatchPlayerTimerProvider(IPlayerTimeSpanProvider playerTimeSpanProvider)
    {
        _playerTimeSpanProvider = playerTimeSpanProvider;
    }

    public IPlayerTimer GetPlayerTimer()
    {
        var timeSpan = _playerTimeSpanProvider.GetTimeSpan();
        return new StopwatchPlayerTimer(timeSpan);
    }
}