using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public class GameCreator : IGameCreator
{
    private readonly IPlayersListProvider _playersListProvider;
    private readonly IBoardConfigProvider _boardConfigProvider;
    private readonly IPlaceablesConfigProvider _placeablesConfigProvider;
    private readonly IPlayersDecksProvider _playersDecksProvider;

    public GameCreator(IPlayersListProvider playersListProvider, IBoardConfigProvider boardConfigProvider, IPlaceablesConfigProvider placeablesConfigProvider, IPlayersDecksProvider playersDecksProvider)
    {
        _playersListProvider = playersListProvider;
        _boardConfigProvider = boardConfigProvider;
        _placeablesConfigProvider = placeablesConfigProvider;
        _playersDecksProvider = playersDecksProvider;
    }

    public Game CreateGame(List<int> playersIds)
    {
        throw new NotImplementedException();
    }
}