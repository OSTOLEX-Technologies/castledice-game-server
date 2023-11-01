using castledice_game_logic;

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
        throw new NotImplementedException();
    }
}