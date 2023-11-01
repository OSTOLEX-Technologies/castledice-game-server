using castledice_game_logic;
using castledice_game_server.Exceptions;

namespace castledice_game_server.GameController.Moves;

public class GameForPlayerProvider : IGameForPlayerProvider
{
    private readonly IGamesCollection _gamesCollection;

    public GameForPlayerProvider(IGamesCollection gamesCollection)
    {
        _gamesCollection = gamesCollection;
    }

    public Game GetGame(int playerId)
    {
        var game = _gamesCollection.FirstOrDefault(g => g.GetAllPlayersIds().Any(id => id == playerId));
        if (game == null)
        {
            throw new GameNotFoundException("Game with given player id does not exists. Player id: " + playerId + ".");
        }
        return game;
    }
}