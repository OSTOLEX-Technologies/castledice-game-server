namespace castledice_game_server.Auth;

public interface IIdRetriever
{
    int RetrievePlayerId(string playerToken);
}