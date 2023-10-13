using castledice_game_logic;

namespace castledice_game_server.GameController;

public class ActiveGamesCollection
{
    public List<Game> ActiveGames { get; } = new();
}