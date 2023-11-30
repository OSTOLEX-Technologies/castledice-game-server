using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscProviders;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameCreator : IGameCreator
{
    private readonly IPlayersListProvider _playersListProvider;
    private readonly IBoardConfigProvider _boardConfigProvider;
    private readonly IPlaceablesConfigProvider _placeablesConfigProvider;
    private readonly ITscListProvider _tscListProvider;
    private readonly IPlayersDecksProvider _playersDecksProvider;
    private readonly IGameConstructorWrapper _gameConstructorWrapper;

    public GameCreator(IPlayersListProvider playersListProvider, IBoardConfigProvider boardConfigProvider, IPlaceablesConfigProvider placeablesConfigProvider, ITscListProvider tscListProvider, IPlayersDecksProvider playersDecksProvider, IGameConstructorWrapper gameConstructorWrapper)
    {
        _playersListProvider = playersListProvider;
        _boardConfigProvider = boardConfigProvider;
        _placeablesConfigProvider = placeablesConfigProvider;
        _tscListProvider = tscListProvider;
        _playersDecksProvider = playersDecksProvider;
        _gameConstructorWrapper = gameConstructorWrapper;
    }

    public Game CreateGame(List<int> playersIds)
    {
        var players = _playersListProvider.GetPlayersList(playersIds);
        var boardConfig = _boardConfigProvider.GetBoardConfig(players);
        var placeablesConfig = _placeablesConfigProvider.GetPlaceablesConfig();
        var decksList = _playersDecksProvider.GetPlayersDecksList(playersIds);
        var game = _gameConstructorWrapper.ConstructGame(players, boardConfig, placeablesConfig, decksList);
        var currentPlayerProvider = game.CurrentPlayerProvider;
        var tscList = _tscListProvider.GetTurnSwitchConditions();
        game.AddTurnSwitchConditionsList(tscList);
        return game;
    }
}