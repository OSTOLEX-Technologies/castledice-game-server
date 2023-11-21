namespace castledice_game_server.GameService;

public class CurrentTimeProvider : ICurrentTimeProvider
{
    public DateTime GetCurrentTime()
    {
        return DateTime.Now;
    }
}