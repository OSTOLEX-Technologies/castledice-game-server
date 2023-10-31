namespace castledice_game_server.Exceptions;

public class GameDataNotFoundException : Exception
{
    public GameDataNotFoundException()
    {
    }

    public GameDataNotFoundException(string? message) : base(message)
    {
    }
}