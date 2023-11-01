using castledice_game_logic;

namespace castledice_game_server.GameController;

public interface IGamesCollection : IEnumerable<Game>
{
    void AddGame(int gameId, Game game);
    int GetGameId(Game game);
    bool RemoveGame(int gameId);
    event EventHandler<Game> GameAdded;
    event EventHandler<Game> GameRemoved;
}