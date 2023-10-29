using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation;

public class GameStartDataCreator : IGameStartDataCreator
{
    private readonly IGameStartDataVersionProvider _versionProvider;
    private readonly IBoardDataProvider _boardDataProvider;
    private readonly IPlaceablesConfigDataProvider _placeablesConfigDataProvider;
    private readonly IDecksDataProvider _decksDataProvider;

    public GameStartDataCreator(IGameStartDataVersionProvider versionProvider, IBoardDataProvider boardDataProvider, IPlaceablesConfigDataProvider placeablesConfigDataProvider, IDecksDataProvider decksDataProvider)
    {
        _versionProvider = versionProvider;
        _boardDataProvider = boardDataProvider;
        _placeablesConfigDataProvider = placeablesConfigDataProvider;
        _decksDataProvider = decksDataProvider;
    }

    public GameStartData CreateGameStartData(Game game)
    {
        throw new NotImplementedException();
    }
}