using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscConfigProviders;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameCreator : IGameCreator
{
    private readonly IPlayersListProvider _playersListProvider;
    private readonly IBoardConfigProvider _boardConfigProvider;
    private readonly IPlaceablesConfigProvider _placeablesConfigProvider;
    private readonly ITscConfigProvider _tscConfigProvider;
    private readonly IPlayersDecksProvider _playersDecksProvider;
    private readonly IGameConstructorWrapper _gameConstructorWrapper;

    public GameCreator(IPlayersListProvider playersListProvider, IBoardConfigProvider boardConfigProvider, IPlaceablesConfigProvider placeablesConfigProvider,  IPlayersDecksProvider playersDecksProvider, ITscConfigProvider tscConfigProvider, IGameConstructorWrapper gameConstructorWrapper)
    {
        _playersListProvider = playersListProvider;
        _boardConfigProvider = boardConfigProvider;
        _placeablesConfigProvider = placeablesConfigProvider;
        _tscConfigProvider = tscConfigProvider;
        _playersDecksProvider = playersDecksProvider;
        _gameConstructorWrapper = gameConstructorWrapper;
    }

    public Game CreateGame(List<int> playersIds)
    {
        var players = _playersListProvider.GetPlayersList(playersIds);
        var boardConfig = _boardConfigProvider.GetBoardConfig(players);
        var placeablesConfig = _placeablesConfigProvider.GetPlaceablesConfig();
        var decksList = _playersDecksProvider.GetPlayersDecksList(playersIds);
        var tscConfig = _tscConfigProvider.GetTurnSwitchConditionsConfig();
        var game = _gameConstructorWrapper.ConstructGame(players, boardConfig, placeablesConfig, decksList, tscConfig);
        return game;
    }
}