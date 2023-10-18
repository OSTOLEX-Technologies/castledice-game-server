﻿using castledice_game_server.GameDataSaver;
using castledice_game_server.NetworkManager;

namespace castledice_game_server.GameController;

public class GameInitializationController : IGameInitializationController
{
    private IGameSavingService _gameSavingService;
    private ActiveGamesCollection _activeGamesCollection;
    private IGameStartDataSender _gameStartDataSender;
    private IGameCreator _gameCreator;
    private IGameStartDataCreator _gameStartDataCreator;

    public GameInitializationController(IGameSavingService gameSavingService, ActiveGamesCollection activeGamesCollection, IGameStartDataSender gameStartDataSender, IGameCreator gameCreator, IGameStartDataCreator gameStartDataCreator)
    {
        _gameSavingService = gameSavingService;
        _activeGamesCollection = activeGamesCollection;
        _gameStartDataSender = gameStartDataSender;
        _gameCreator = gameCreator;
        _gameStartDataCreator = gameStartDataCreator;
    }

    public void InitializeGame(List<int> playersIds)
    {
        var game = _gameCreator.CreateGame(playersIds);
        _activeGamesCollection.ActiveGames.Add(game);
        _gameSavingService.SaveGameStart(game);
        var gameStartData = _gameStartDataCreator.CreateGameStartData(game);
        _gameStartDataSender.SendGameStartData(gameStartData);
    }
}