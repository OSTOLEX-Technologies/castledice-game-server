namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

public class DefaultPlayerTimeSpanProvider :IPlayerTimeSpanProvider
{
    private readonly TimeSpan _defaultTimeSpan;

    public DefaultPlayerTimeSpanProvider(TimeSpan defaultTimeSpan)
    {
        _defaultTimeSpan = defaultTimeSpan;
    }

    public TimeSpan GetTimeSpan()
    {
        return _defaultTimeSpan;
    }
}