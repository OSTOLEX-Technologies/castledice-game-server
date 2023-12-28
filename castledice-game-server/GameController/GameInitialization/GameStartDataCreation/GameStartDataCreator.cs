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
    private readonly IDecksDataProvider _decksDataProvider;

    public GameStartDataCreator(IGameStartDataVersionProvider versionProvider, IBoardDataProvider boardDataProvider, IPlaceablesConfigDataProvider placeablesConfigDataProvider, ITscConfigDataProvider tscConfigDataProvider, IDecksDataProvider decksDataProvider)
    {
        _versionProvider = versionProvider;
        _boardDataProvider = boardDataProvider;
        _placeablesConfigDataProvider = placeablesConfigDataProvider;
        _tscConfigDataProvider = tscConfigDataProvider;
        _decksDataProvider = decksDataProvider;
    }

    public GameStartData CreateGameStartData(Game game)
    {
        var version = _versionProvider.GetGameStartDataVersion();
        var boardData = _boardDataProvider.GetBoardData(game.GetBoard());
        var placeablesConfigData = _placeablesConfigDataProvider.GetPlaceablesConfigData(game.PlaceablesConfig);
        var playersDecksData = _decksDataProvider.GetPlayersDecksData(game.GetDecksList(), game.GetAllPlayersIds());
        var tscConfigData = _tscConfigDataProvider.GetTscConfigData(game.TurnSwitchConditionsConfig);
        var playersIds = game.GetAllPlayersIds();
        
        return new GameStartData(version, boardData, placeablesConfigData, tscConfigData, playersIds, playersDecksData);
    }
}