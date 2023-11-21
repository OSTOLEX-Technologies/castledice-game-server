namespace castledice_game_server.Auth;

public interface IIdRetriever
{
    Task<int> RetrievePlayerIdAsync(string playerToken);
}