using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators.ContentSpawnersCreators.CastlesSpawning;

public class DuelCastlesSpawnerCreator : ICastlesSpawnerCreator
{
    private readonly IDuelCastlesPositionsCreator _positionsCreator;
    private readonly ICastlesFactoryCreator _factoryCreator;

    public DuelCastlesSpawnerCreator(IDuelCastlesPositionsCreator positionsCreator, ICastlesFactoryCreator factoryCreator)
    {
        _positionsCreator = positionsCreator;
        _factoryCreator = factoryCreator;
    }

    /// <summary>
    /// This implementation of this method accepts only lists of two players. If the number of players is other than two - an exception is thrown.
    /// First player from the list is considered first player on board, second player from the list is considered second player on board.
    /// </summary>
    /// <param name="players"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public CastlesSpawner GetCastlesSpawner(List<Player> players)
    {
        if (players.Count != 2) throw new ArgumentException("Duel castles spawner accepts only two players.");
        var factory = _factoryCreator.GetCastlesFactory();
        var firstPlayerPosition = _positionsCreator.GetFirstCastlePosition();
        var secondPlayerPosition = _positionsCreator.GetSecondCastlePosition();
        var playersToCastlePositions = new Dictionary<Player, Vector2Int>
        {
            {players[0], firstPlayerPosition},
            {players[1], secondPlayerPosition}
        };
        return new CastlesSpawner(playersToCastlePositions, factory);
    }
}