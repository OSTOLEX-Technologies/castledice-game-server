using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameCreator : IGameCreator
{
    private readonly IPlayersListCreator _playersListCreator;
    private readonly IBoardConfigCreator _boardConfigCreator;
    private readonly IPlaceablesConfigCreator _placeablesConfigCreator;
    private readonly ITscConfigCreator _tscConfigCreator;
    private readonly IGameConstructorWrapper _gameConstructorWrapper;

    public GameCreator(IPlayersListCreator playersListCreator, IBoardConfigCreator boardConfigCreator, IPlaceablesConfigCreator placeablesConfigCreator, ITscConfigCreator tscConfigCreator, IGameConstructorWrapper gameConstructorWrapper)
    {
        _playersListCreator = playersListCreator;
        _boardConfigCreator = boardConfigCreator;
        _placeablesConfigCreator = placeablesConfigCreator;
        _tscConfigCreator = tscConfigCreator;
        _gameConstructorWrapper = gameConstructorWrapper;
    }

    public Game CreateGame(List<int> playersIds)
    {
        var players = _playersListCreator.GetPlayersList(playersIds);
        var boardConfig = _boardConfigCreator.GetBoardConfig(players);
        var placeablesConfig = _placeablesConfigCreator.GetPlaceablesConfig();
        var tscConfig = _tscConfigCreator.GetTurnSwitchConditionsConfig();
        var game = _gameConstructorWrapper.ConstructGame(players, boardConfig, placeablesConfig, tscConfig);
        return game;
    }
}