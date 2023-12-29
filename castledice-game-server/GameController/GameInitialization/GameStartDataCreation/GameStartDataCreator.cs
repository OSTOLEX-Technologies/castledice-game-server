using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation;

public class GameStartDataCreator : IGameStartDataCreator
{
    private readonly IGameStartDataVersionProvider _versionProvider;
    private readonly IBoardDataProvider _boardDataProvider;
    private readonly IPlaceablesConfigDataProvider _placeablesConfigDataProvider;
    private readonly ITscConfigDataProvider _tscConfigDataProvider;
    private readonly IPlayersDataListCreator _playersDataListCreator;

    public GameStartDataCreator(IGameStartDataVersionProvider versionProvider, IBoardDataProvider boardDataProvider, IPlaceablesConfigDataProvider placeablesConfigDataProvider, ITscConfigDataProvider tscConfigDataProvider, IPlayersDataListCreator playersDataListCreator)
    {
        _versionProvider = versionProvider;
        _boardDataProvider = boardDataProvider;
        _placeablesConfigDataProvider = placeablesConfigDataProvider;
        _tscConfigDataProvider = tscConfigDataProvider;
        _playersDataListCreator = playersDataListCreator;
    }

    public GameStartData CreateGameStartData(Game game)
    {
        var version = _versionProvider.GetGameStartDataVersion();
        var boardData = _boardDataProvider.GetBoardData(game.GetBoard());
        var placeablesConfigData = _placeablesConfigDataProvider.GetPlaceablesConfigData(game.PlaceablesConfig);
        var tscConfigData = _tscConfigDataProvider.GetTscConfigData(game.TurnSwitchConditionsConfig);
        
        return new GameStartData(version, boardData, placeablesConfigData, tscConfigData, null);
    }
}