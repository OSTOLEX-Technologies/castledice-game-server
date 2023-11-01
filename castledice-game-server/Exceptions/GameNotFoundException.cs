namespace castledice_game_server.Exceptions;

public class GameNotFoundException : Exception
{
    public GameNotFoundException()
    {
    }
    
    public GameNotFoundException(string message) : base(message)
    {
    }
}