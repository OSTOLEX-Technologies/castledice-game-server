namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;

public class DefaultPlayerTimeSpanCreator :IPlayerTimeSpanCreator
{
    private readonly TimeSpan _defaultTimeSpan;

    public DefaultPlayerTimeSpanCreator(TimeSpan defaultTimeSpan)
    {
        _defaultTimeSpan = defaultTimeSpan;
    }

    public TimeSpan GetTimeSpan()
    {
        return _defaultTimeSpan;
    }
}