﻿using System.Collections;
using castledice_game_logic;

namespace castledice_game_server.GameController;

public class ActiveGamesCollection : IGamesCollection
{
    private readonly Dictionary<int, Game> _games = new();
    
    public IEnumerator<Game> GetEnumerator()
    {
        return _games.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddGame(int gameId, Game game)
    {
        if (_games.ContainsKey(gameId))
        {
            throw new ArgumentException("Game with id " + gameId + " already exists");
        }
        if (_games.ContainsValue(game))
        {
            throw new ArgumentException("Given game instance already exists");
        }
        _games.Add(gameId, game);
        GameAdded?.Invoke(this, game);
    }

    public int GetGameId(Game game)
    {
        if (!_games.ContainsValue(game))
        {
            throw new ArgumentException("Given game instance does not exists");
        }
        return _games.First(x => x.Value == game).Key;
    }

    public bool RemoveGame(int gameId)
    {
        if (!_games.ContainsKey(gameId)) return false;
        var game = _games[gameId];
        GameRemoved?.Invoke(this, game);
        return _games.Remove(gameId);
    }

    public event EventHandler<Game>? GameAdded;
    public event EventHandler<Game>? GameRemoved;
}