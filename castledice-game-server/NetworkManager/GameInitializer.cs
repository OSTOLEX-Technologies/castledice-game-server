using casltedice_events_logic.ServerToClient;
using castledice_game_server.GameController;

namespace castledice_game_server.NetworkManager;

public class GameInitializer : IMatchFoundDTOAccepter
{
    private IGameInitializationController _gameInitializationController;

    public GameInitializer(IGameInitializationController gameInitializationController)
    {
        _gameInitializationController = gameInitializationController;
    }

    public void AcceptMatchFoundDTO(MatchFoundDTO matchFoundDTO)
    {
        _gameInitializationController.InitializeGame(matchFoundDTO.PlayerIds);
    }
}