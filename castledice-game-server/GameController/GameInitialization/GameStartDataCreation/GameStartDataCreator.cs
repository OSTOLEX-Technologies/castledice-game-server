using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation;

public class GameStartDataCreator : IGameStartDataCreator
{
    private readonly IGameStartDataVersionProvider _versionProvider;
    private readonly IBoardDataCreator _boardDataCreator;
    private readonly IPlaceablesConfigDataCreator _placeablesConfigDataCreator;
    private readonly ITscConfigDataCreator _tscConfigDataCreator;
    private readonly IPlayersDataListCreator _playersDataListCreator;

    public GameStartDataCreator(IGameStartDataVersionProvider versionProvider, IBoardDataCreator boardDataCreator, IPlaceablesConfigDataCreator placeablesConfigDataCreator, ITscConfigDataCreator tscConfigDataCreator, IPlayersDataListCreator playersDataListCreator)
    {
        _versionProvider = versionProvider;
        _boardDataCreator = boardDataCreator;
        _placeablesConfigDataCreator = placeablesConfigDataCreator;
        _tscConfigDataCreator = tscConfigDataCreator;
        _playersDataListCreator = playersDataListCreator;
    }

    public GameStartData CreateGameStartData(Game game)
    {
        var version = _versionProvider.GetGameStartDataVersion();
        var boardData = _boardDataCreator.GetBoardData(game.GetBoard());
        var placeablesConfigData = _placeablesConfigDataCreator.GetPlaceablesConfigData(game.PlaceablesConfig);
        var tscConfigData = _tscConfigDataCreator.GetTscConfigData(game.TurnSwitchConditionsConfig);
        var playersData = _playersDataListCreator.GetPlayersData(game.GetAllPlayers());
        
        return new GameStartData(version, boardData, placeablesConfigData, tscConfigData, playersData);
    }
}