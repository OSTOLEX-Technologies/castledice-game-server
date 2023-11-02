using System.Collections;
using castledice_game_logic;
using castledice_game_server.GameController;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.TestImplementations;

public class TestGamesCollection : IGamesCollection
{
    public Game GameToReturnOnGameRemoved { get; set; } = GetGame();
        
    public IEnumerator<Game> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddGame(int gameId, Game game)
    {
        GameAdded?.Invoke(this, game);
    }

    public int GetGameId(Game game)
    {
        return 1;
    }

    public bool RemoveGame(int gameId)
    {
        GameRemoved?.Invoke(this, GameToReturnOnGameRemoved);
        return true;
    }

    public event EventHandler<Game>? GameAdded;
    public event EventHandler<Game>? GameRemoved;
}