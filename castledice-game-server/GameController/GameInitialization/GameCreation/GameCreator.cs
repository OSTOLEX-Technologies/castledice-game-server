﻿using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameCreator : IGameCreator
{
    private readonly IPlayersListProvider _playersListProvider;
    private readonly IBoardConfigProvider _boardConfigProvider;
    private readonly IPlaceablesConfigProvider _placeablesConfigProvider;
    private readonly IPlayersDecksProvider _playersDecksProvider;
    private readonly IGameConstructorWrapper _gameConstructorWrapper;

    public GameCreator(IPlayersListProvider playersListProvider, IBoardConfigProvider boardConfigProvider, IPlaceablesConfigProvider placeablesConfigProvider, IPlayersDecksProvider playersDecksProvider, IGameConstructorWrapper gameConstructorWrapper)
    {
        _playersListProvider = playersListProvider;
        _boardConfigProvider = boardConfigProvider;
        _placeablesConfigProvider = placeablesConfigProvider;
        _playersDecksProvider = playersDecksProvider;
        _gameConstructorWrapper = gameConstructorWrapper;
    }

    public Game CreateGame(List<int> playersIds)
    {
        throw new NotImplementedException();
    }
}