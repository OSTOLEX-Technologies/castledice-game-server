﻿using castledice_game_logic;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.Math;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.ContentSpawnersProviders.CastlesSpawning;

public class DuelCastlesSpawnerProvider : ICastlesSpawnerProvider
{
    private readonly IDuelCastlesPositionsProvider _positionsProvider;
    private readonly ICastlesFactoryProvider _factoryProvider;

    public DuelCastlesSpawnerProvider(IDuelCastlesPositionsProvider positionsProvider, ICastlesFactoryProvider factoryProvider)
    {
        _positionsProvider = positionsProvider;
        _factoryProvider = factoryProvider;
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
        var factory = _factoryProvider.GetCastlesFactory();
        var firstPlayerPosition = _positionsProvider.GetFirstCastlePosition();
        var secondPlayerPosition = _positionsProvider.GetSecondCastlePosition();
        var playersToCastlePositions = new Dictionary<Player, Vector2Int>
        {
            {players[0], firstPlayerPosition},
            {players[1], secondPlayerPosition}
        };
        return new CastlesSpawner(playersToCastlePositions, factory);
    }
}